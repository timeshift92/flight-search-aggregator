using Aggregator.Data;
using Riok.Mapperly.Abstractions;

namespace Aggregator.Services;

[Mapper]
public static partial class FlightMapper
{
    #region Usable

    public static FlightView MapToView(this FlightEntity src) => src.To();
    public static List<FlightView> MapToViewList(this List<FlightEntity> src) => src.ToList();
    public static FlightEntity MapFromView(this FlightView src) => src.From();

    #endregion

    #region Internal

    private static partial FlightView To(this FlightEntity src);
    private static partial List<FlightView> ToList(this List<FlightEntity> src);
    private static partial FlightEntity From(this FlightView TodoView);
    public static partial void From(FlightView personView, FlightEntity personEntity);

    #endregion
}