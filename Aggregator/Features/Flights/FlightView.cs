using System.Runtime.Serialization;
using ActualLab.Fusion.Blazor;
using MemoryPack;

namespace Aggregator.Services;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FlightView
{
    [property: DataMember] public long Id { get; }

    [property: DataMember] public string TicketNumber { get; set; } = null!;
    [property: DataMember] public decimal Price { get; init; }
    [property: DataMember] public string Airline { get; set; } = null!;
    [property: DataMember] public string DepartureCity { get; set; } = null!;
    [property: DataMember] public DateTime DepartureTime { get; init; }
    [property: DataMember] public string ArrivalCity { get; set; } = null!;
    [property: DataMember] public DateTime ArrivalTime { get; init; }
    [property: DataMember] public int Seats { get; set; }
    [property: DataMember] public int FlightCount { get; set; }
    [property: DataMember] public FlightServiceEnum Service { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as FlightView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}