using Aggregator.Data;
using Riok.Mapperly.Abstractions;

namespace Aggregator.Services;

[Mapper]
public static partial class FlightMapper
{
    #region Usable

    
    public static FlightView MapToView(this FlightEntity src) => src.To();
    public static List<FlightView> MapToViewList(this List<FlightEntity> src) => src.ToList();

    #endregion

    #region Internal

    [MapProperty("Id","Id")]
    private static partial FlightView To(this FlightEntity src);
    [MapProperty("Id","Id")]
    private static partial List<FlightView> ToList(this List<FlightEntity> src);
    
    #endregion
}