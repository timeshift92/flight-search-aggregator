using System.Globalization;
using ActualLab.CommandR;
using Aggregator.Data;
using Aggregator.Services;
using Coravel.Invocable;
using MudBlazor.Extensions;

namespace Aggregator.HostedServices.FlyZen;

public class FlyZenServiceScheduler(
    IHttpClientFactory httpClientFactory,
    AppDbContext context,
    ILogger<FlyZenServiceScheduler> logger,
    ICommander commander) : IInvocable
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("FlyZenService");

    public async Task Invoke()
    {
        logger.LogInformation("FlyZenServiceScheduler Started");

        try
        {
            var current = DateTime.Now;

            var today = current.StartOfMonth(new CultureInfo("en")).ToString("yyyy-M-d");
            var thirtyDays = current.AddDays(30).ToString("yyyy-M-d");
            var result = await _client.GetFromJsonAsync<List<Ticket>>($"/tickets?from={today}&to={thirtyDays}");

            if (result != null)
            {
                HashSet<string> arrivalCities = [];
                HashSet<string> departureCities = [];
                HashSet<string> airlines = [];
                var flights = result.ToFlights();
                flights.ForEach(x =>
                {
                    arrivalCities.Add(x.ArrivalCity);
                    departureCities.Add(x.DepartureCity);
                    airlines.Add(x.Airline);
                });

                context.Flights.AddRange(flights);
                await context.SaveChangesAsync();
                await commander.Call(new SetAirlinesCommand(airlines));
                await commander.Call(new SetCitiesCommand(arrivalCities, CityTypeEnum.Arrival));
                await commander.Call(new SetCitiesCommand(departureCities, CityTypeEnum.Departure));
                await commander.Call(new SetFlightIdsCommand(context.Flights.ToList().MapToViewList()));
            }

            logger.LogInformation("FlyZenServiceScheduler Completed");
        }
        catch (Exception e)
        {
            logger.LogError("FlyZenServiceScheduler Error {EMessage}", e.Message);
            throw;
        }
    }
}