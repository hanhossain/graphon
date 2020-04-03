using CoreGraphics;
using Graphon.Core;

namespace Graphon.iOS.Extensions
{
    public static class ChartEntryExtensions
    {
        public static CGPoint AsPoint<Tx, Ty>(this ChartEntry<Tx, Ty> entry, IChartAxisSource<Tx, Ty> chartAxisSource)
            where Tx : struct
            where Ty : struct
        {
            double x = chartAxisSource.MapToXCoordinate(entry.X);
            double y = chartAxisSource.MapToYCoordinate(entry.Y);
            return new CGPoint(x, y);
        }
    }
}