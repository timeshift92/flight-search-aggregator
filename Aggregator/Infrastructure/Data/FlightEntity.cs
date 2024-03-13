using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aggregator.Services;
using Microsoft.EntityFrameworkCore;

namespace Aggregator.Data;

[Index(nameof(Airline))]
[Index(nameof(DepartureCity))]
[Index(nameof(DepartureTime))]
[Index(nameof(ArrivalCity))]
[Index(nameof(ArrivalTime))]
[Index(nameof(Price))]
[Index(nameof(Seats))]
[PrimaryKey(nameof(Id))]
public class FlightEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [MaxLength(70)]
    public string TicketNumber { get; set; } = null!;
    public decimal Price { get; init; }
    [MaxLength(40)]
    public string Airline { get; set; } = null!;
    [MaxLength(40)]
    public string DepartureCity { get; set; } = null!;
    public DateTime DepartureTime { get; init; }
    [MaxLength(40)]
    public string ArrivalCity { get; set; } = null!;
    public DateTime ArrivalTime { get; init; }
    public int Seats { get; set; }
    public int FlightCount { get; set; }
    public FlightServiceEnum Service { get; set; }
}