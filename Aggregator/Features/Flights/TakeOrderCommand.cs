using System.Runtime.Serialization;
using ActualLab.CommandR;
using MemoryPack;

namespace Aggregator.Services;


[DataContract, MemoryPackable]
public partial  record TakeOrderCommand([property: DataMember] long FlightId) : ICommand<Task>;

[DataContract, MemoryPackable]
public partial  record SetFlightIdsCommand([property: DataMember] List<FlightView> FlightViews) : ICommand<Task>;
[DataContract, MemoryPackable]
public partial  record SetAirlinesCommand([property: DataMember] HashSet<string> Airlines) : ICommand<Task>;
[DataContract, MemoryPackable]
public partial  record SetCitiesCommand([property: DataMember] HashSet<string> Cities,[property: DataMember] CityTypeEnum Type) : ICommand<Task>;

