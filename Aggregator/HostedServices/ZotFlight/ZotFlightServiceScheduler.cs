using Aggregator.Data;
using Aggregator.HostedServices.ZotFlightModels;
using Aggregator.Infrastructure.Mapper;
using Coravel.Invocable;

namespace Aggregator.HostedServices;

public class ZotFlightServiceScheduler(
    IHttpClientFactory httpClientFactory,
    AppDbContext context,
    ILogger<ZotFlightServiceScheduler> logger) : IInvocable
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("ZotFlightService");

    public async Task Invoke()
    {
        logger.LogInformation("ZotFlightServiceScheduler Started");

        try
        {
            var current = DateTime.Now;
            var lastFlight = context.Flights.MaxBy(x => x.ArrivalTime);
            if (lastFlight is not null)
            {
                current = lastFlight.ArrivalTime;
            }

            var today = current.ToString("yyyy-M-d");
            var tenDays = current.AddDays(10).ToString("yyyy-M-d");
            var result = await _client.GetFromJsonAsync<List<Ticket>>($"/flights?from={today}&to={tenDays}");
            if (result != null)
            {
                context.Flights.AddRange(result.ToFlights());
                await context.SaveChangesAsync();
            }

            logger.LogInformation("ZotFlightServiceScheduler Completed");
        }
        catch (Exception e)
        {
            logger.LogError("ZotFlightServiceScheduler Error {EMessage}", e.Message);
            throw;
        }
    }
}