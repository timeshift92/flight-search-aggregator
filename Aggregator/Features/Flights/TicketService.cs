using System.Reactive;
using ActualLab.Async;
using ActualLab.Collections;
using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using Aggregator.Data;

namespace Aggregator.Services;

public class TicketService(DbHub<AppDbContext> dbHub) : ITicketService
{
    private static HashSet<long> _flightIds = [];
    private static HashSet<string> _arrivalCities = [];
    private static HashSet<string> _departureCities = [];
    private static HashSet<string> _airlines = [];
    private static List<FlightView> _flightViews = [];

    public virtual async Task<HashSet<string>> GetCities(string? search, CityTypeEnum type = CityTypeEnum.Arrival,
        CancellationToken cancellationToken = default)
    {
        await InvalidateCities();
        var _cities = type == CityTypeEnum.Arrival ? _arrivalCities : _departureCities;
        if (_cities.Count == 0)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var cities = dbContext.Flights.Select(x => new { x.DepartureCity, x.ArrivalCity }).ToList();
            cities.ForEach(x =>
            {
                _cities.Add(x.DepartureCity);
                _cities.Add(x.ArrivalCity);
            });
        }

        return search is null
            ? _cities
            : _cities.Where(x => x.Contains(search, StringComparison.CurrentCultureIgnoreCase)).ToHashSet();
    }

    public virtual async Task<HashSet<string>> GetAirlines(string? search,
        CancellationToken cancellationToken = default)
    {
        await InvalidateAirlines();
        if (_airlines.Count == 0)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            _airlines = dbContext.Flights.Select(x => x.Airline).ToHashSet();
        }

        return search is null
            ? _airlines
            : _airlines.Where(x => x.Contains(search, StringComparison.CurrentCultureIgnoreCase)).ToHashSet();
    }

    public virtual async Task<List<FlightView>> GetFlights(FlightQuery? query,
        CancellationToken cancellationToken = default)
    {
        await Invalidate();
        List<FlightView> flights = [];

        flights = await GetFlight(_flightIds.ToArray(), cancellationToken);
        var qry = flights.AsQueryable();
        if (string.IsNullOrEmpty(query.DepartureCity) ^ 
            string.IsNullOrEmpty(query.ArrivalCity) ^
                                    query.ArrivalCity is null  ^ query.DepartureTime is null ^ query.Price == 0 ^ query.Airline is null
            )
        {
            qry = flights.Take(50).AsQueryable();
        };
        
         
        if (!string.IsNullOrEmpty(query.DepartureCity) && query.DepartureTime != null)
        {
            qry = qry.Where(x => x.DepartureCity == query.DepartureCity);
        }
        
        if (query.DepartureTime is not null)
        {
            qry = qry.Where(x => x.DepartureTime >= query.DepartureTime  );
        }
        

        if (!string.IsNullOrEmpty(query.ArrivalCity))
        {
            qry = qry.Where(x => x.ArrivalCity == query.ArrivalCity);
        }

        if (query.ArrivalTime is not null)
        {
            qry = qry.Where(x => x.ArrivalTime <= query.ArrivalTime);
        }

        if (query.Price is not null && query.Price != 0)
        {
            qry = qry.Where(x => x.Price <= query.Price);
        }

        if (query.FlightCount != 0)
        {
            qry = qry.Where(x => x.FlightCount <= query.FlightCount);
        }
        
        foreach (var order in query.OrderBy)
        {
            var sort = order.Split("_");
            qry = sort[0] switch
            {
                "Airline" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.Airline)
                    : qry.OrderByDescending(x => x.Airline),
                "DepartureCity" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.DepartureCity)
                    : qry.OrderByDescending(x => x.DepartureCity),
                "ArrivalCity" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.ArrivalCity)
                    : qry.OrderByDescending(x => x.ArrivalCity),
                "Price" => sort[1] == "asc" ? qry.OrderBy(x => x.Price) : qry.OrderByDescending(x => x.Price),
                "DepartureTime" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.DepartureTime)
                    : qry.OrderByDescending(x => x.DepartureTime),
                "ArrivalTime" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.ArrivalTime)
                    : qry.OrderByDescending(x => x.ArrivalTime),
                "FlightCount" => sort[1] == "asc"
                    ? qry.OrderBy(x => x.FlightCount)
                    : qry.OrderByDescending(x => x.FlightCount),
                _ => qry
            };
        }

        

        return qry.ToList();
    }

    public virtual async Task<List<FlightView>> GetFlight(long[] ids, CancellationToken cancellationToken = default)
    {
        var dbContext = dbHub.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);
        if (ids.Length == 0 && _flightViews.Count == 0)
            _flightViews.AddRange(dbContext.Flights.ToList().MapToViewList());

        return _flightViews;
    }

    public virtual async Task TakeOrder(TakeOrderCommand takeOrderCommand,
        CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            // await GetFlight(takeOrderCommand.FlightId, cancellationToken);
            return;
        }

        throw new NotImplementedException();
    }

    public virtual async Task SetFlightIds(SetFlightIdsCommand setFlightIdsCommand,
        CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            await Invalidate();
            return;
        }

        _flightViews = setFlightIdsCommand.FlightViews;

        _flightIds = setFlightIdsCommand.FlightViews.Select(x => x.Id).ToHashSet();
    }

    public virtual async Task SetAirlines(SetAirlinesCommand setAirlinesCommand,
        CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            await InvalidateAirlines();
            return;
        }

        _airlines.AddRange(setAirlinesCommand.Airlines);
    }

    public virtual async Task SetCities(SetCitiesCommand setCitiesCommand,
        CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            await InvalidateCities();
            return;
        }

        if (setCitiesCommand.Type == CityTypeEnum.Arrival) _arrivalCities.AddRange(setCitiesCommand.Cities);

        _departureCities.AddRange(setCitiesCommand.Cities);
    }

    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    public virtual Task<Unit> InvalidateAirlines() => TaskExt.UnitTask;

    public virtual Task<Unit> InvalidateCities() => TaskExt.UnitTask;
}