using System.Runtime.Serialization;
using MemoryPack;

namespace Aggregator.Services;

[DataContract, MemoryPackable]
public partial record FlightQuery(
    [property: DataMember]string? DepartureCity,
    [property: DataMember]string? ArrivalCity,
    [property: DataMember]DateTime? DepartureTime,
    [property: DataMember]DateTime? ArrivalTime,
    [property: DataMember]decimal? Price,
    [property: DataMember]string? Airline,
    [property: DataMember]int FlightCount,
    [property: DataMember]List<string> OrderBy
);
