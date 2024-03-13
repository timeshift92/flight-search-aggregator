using System.Reactive;
using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using Aggregator.Data;

namespace Aggregator.Services;

public interface ITicketService : IComputeService
{
    [ComputeMethod]
    public Task<HashSet<string>> GetCities(string? search, CityTypeEnum type = CityTypeEnum.Arrival, CancellationToken cancellationToken = default);

    [ComputeMethod]
    public Task<HashSet<string>> GetAirlines(string? search, CancellationToken cancellationToken = default);

    [ComputeMethod]
    public Task<List<FlightView>> GetFlights(FlightQuery? query, CancellationToken cancellationToken = default);

    public Task<List<FlightView>> GetFlight(long[] ids, CancellationToken cancellationToken = default);

    [CommandHandler]
    public Task TakeOrder(TakeOrderCommand takeOrderCommand, CancellationToken cancellationToken = default);

    [CommandHandler]
    public Task SetFlightIds(SetFlightIdsCommand setFlightIdsCommand, CancellationToken cancellationToken = default);

    [CommandHandler]
    public Task SetCities(SetCitiesCommand setCitiesCommand, CancellationToken cancellationToken = default);

    [CommandHandler]
    public Task SetAirlines(SetAirlinesCommand setAirlinesCommand,
        CancellationToken cancellationToken = default);

    [ComputeMethod]
    public Task<Unit> Invalidate();

    [ComputeMethod]
    public Task<Unit> InvalidateAirlines();

    [ComputeMethod]
    public Task<Unit> InvalidateCities();
}