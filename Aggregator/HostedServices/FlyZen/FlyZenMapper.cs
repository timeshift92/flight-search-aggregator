using Aggregator.Data;
using Aggregator.HostedServices.ZotFlight;
using Riok.Mapperly.Abstractions;

namespace Aggregator.HostedServices.FlyZen;

[Mapper]
public static partial class FlyZenMapper
{
    #region Usable

    public static FlightEntity MapTo(this Flight src, Ticket ticket) => src.To(ticket);
    public static List<FlightEntity> MapToList(this List<Flight> src, Ticket ticket) => src.ToList(ticket);

    public static List<FlightEntity> ToFlights(this List<Ticket> src)
    {
        var flights = new List<FlightEntity>();
        foreach (var ticket in src)
        {
            flights.AddRange(ticket.ToFlights());
        }

        return flights;
    }

    public static List<FlightEntity> ToFlights(this Ticket src) => src.Flights.ToList(src);

    #endregion

    #region Internal

    private static FlightEntity To(this Flight src, Ticket ticket) => new()
    {
        DepartureCity = src.DepartureCity,
        DepartureTime = src.DepartureTime,
        ArrivalCity = src.ArrivalCity,
        ArrivalTime = src.ArrivalTime,
        Price = ticket.Price,
        Seats = ticket.SeatingCount,
        Airline = ticket.Airline,
        TicketNumber = ticket.Number,
        Service = FlightService.ZotFlight
        
    };

    private static List<FlightEntity> ToList(this List<Flight> src, Ticket ticket) =>
        src.Select(f => f.To(ticket)).ToList();

    #endregion
}