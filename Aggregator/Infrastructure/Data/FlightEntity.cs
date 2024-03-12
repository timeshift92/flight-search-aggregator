using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Aggregator.Data;

[Index(nameof(Airline))]
[Index(nameof(DepartureCity))]
[Index(nameof(DepartureTime))]
[Index(nameof(ArrivalCity))]
[Index(nameof(ArrivalTime))]
[Index(nameof(Price))]
[Index(nameof(Seats))]
public class FlightEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    private long Id { get; set; }

    public string TicketNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public string Airline { get; set; } = null!;
    public string DepartureCity { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public string ArrivalCity { get; set; } = null!;
    public DateTime ArrivalTime { get; set; }
    public int Seats { get; set; }
}