using System.Collections.Generic;

namespace Graphon.iOS
{
    public interface IChartDataSource
    {
        IEnumerable<LineData> GetChartData();
    }
}
