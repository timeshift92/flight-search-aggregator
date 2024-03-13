using System.Reactive;
using Aggregator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TicketController(ITicketService ticketServiceImplementation)
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
    public Task TakeOrder([FromBody] TakeOrderCommand takeOrderCommand, CancellationToken cancellationToken = default)
    {
        return ticketServiceImplementation.TakeOrder(takeOrderCommand, cancellationToken);
    }
}