using System.Reactive;
using ActualLab.CommandR;
using Aggregator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TicketController(ITicketService ticketServiceImplementation,ICommander commander)
{

    [HttpGet("cities")]
    public Task<HashSet<string>> GetFlights([FromQuery] string? search,[FromQuery]  CityTypeEnum cityTypeEnum = CityTypeEnum.Arrival,  CancellationToken cancellationToken = default)
    {
        return ticketServiceImplementation.GetCities(search,cityTypeEnum, cancellationToken);
    }

    [HttpGet("flights")]
    public Task<List<FlightView>> GetFlights([FromQuery] FlightQuery? query, CancellationToken cancellationToken = default)
    {
        return ticketServiceImplementation.GetFlights(query, cancellationToken);
    }
    [HttpPost("take-order")]
    public async Task TakeOrder([FromBody] TakeOrderCommand takeOrderCommand, CancellationToken cancellationToken = default)
    {
        await commander.Call(takeOrderCommand,cancellationToken);
        
    }
}