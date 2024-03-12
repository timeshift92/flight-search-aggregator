using Aggregator.Data;
using Aggregator.HostedServices.ZotFlight;
using Aggregator.Infrastructure.Mapper;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;

namespace Aggregator.HostedServices.FlyZen;

public class FlyZenServiceScheduler(
    IHttpClientFactory httpClientFactory,
    AppDbContext context,
    ILogger<FlyZenServiceScheduler> logger) : IInvocable
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("FlyZenService");

    public async Task Invoke()
    {
        logger.LogInformation("FlyZenServiceScheduler Started");

        try
        {
            var current = DateTime.Now;

            var today = current.ToString("yyyy-M-d");
            var tenDays = current.AddDays(10).ToString("yyyy-M-d");
            var result = await _client.GetFromJsonAsync<List<Ticket>>($"/tickets?from={today}&to={tenDays}");
            if (result != null)
            {
                context.Flights.AddRange(result.ToFlights());
                await context.SaveChangesAsync();
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