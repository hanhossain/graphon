using Foundation;
using Graphon.iOS;
using UIKit;

namespace Graphon.iOS
{
    public interface IChartDataSource<Tx, Ty>
        where Tx : struct
        where Ty : struct
    {
        int NumberOfLines();

        int NumberOfPoints(int lineIndex);

        UIColor GetLineColor(int lineIndex);

        ChartEntry<Tx, Ty> GetEntry(NSIndexPath indexPath);
    }
}
