using System.Runtime.Serialization;
using MemoryPack;

namespace Aggregator.Services;

[DataContract, MemoryPackable]
public partial record FlightQuery(
    [property: DataMember]string DepartureCity,
    [property: DataMember]string ArrivalCity,
    [property: DataMember]DateTime? DepartureTime,
    [property: DataMember]DateTime? ArrivalTime,
    [property: DataMember]decimal? Price,
    [property: DataMember]string? Airline,
    [property: DataMember]int FlightCount,
    [property: DataMember]List<string> OrderBy
);


// public partial class FlightQuery(
//     string DepartureCity,
//     string ArrivalCity,
//     DateTime? DepartureTime,
//     DateTime? ArrivalTime,
//     decimal? Price,
//     string? Airline,
//     int FlightCount,
//     List<string> OrderBy
// )
// {
//     [property: DataMember] public string DepartureCity { get; set; } = DepartureCity;
//     [property: DataMember] public string ArrivalCity { get; set; } = ArrivalCity;
//     [property: DataMember] public DateTime? DepartureTime { get; set; } = DepartureTime;
//     [property: DataMember] public DateTime? ArrivalTime { get; set; } = ArrivalTime;
//     [property: DataMember] public decimal? Price { get; set; } = Price;
//     [property: DataMember] public string? Airline { get; set; } = Airline;
//     [property: DataMember] public int FlightCount { get; set; } = FlightCount;
//     [property: DataMember] public List<string> OrderBy { get; set; } = OrderBy;
//
//     protected bool Equals(FlightQuery other)
//     {
//         return DepartureCity == other.DepartureCity && ArrivalCity == other.ArrivalCity && Nullable.Equals(DepartureTime, other.DepartureTime) && Nullable.Equals(ArrivalTime, other.ArrivalTime) && Price == other.Price && Airline == other.Airline && FlightCount == other.FlightCount && OrderBy.Equals(other.OrderBy);
//     }
//
//     public override bool Equals(object? obj)
//     {
//         if (ReferenceEquals(null, obj)) return false;
//         if (ReferenceEquals(this, obj)) return true;
//         if (obj.GetType() != this.GetType()) return false;
//         return Equals((FlightQuery)obj);
//     }
//
//     public override int GetHashCode()
//     {
//         return HashCode.Combine(DepartureCity, ArrivalCity, DepartureTime, ArrivalTime, Price, Airline, FlightCount, OrderBy);
//     }
// }