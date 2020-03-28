using System.Collections.Generic;

namespace Graphon.iOS
{
    public interface ILineChartDataSource
    {
        IEnumerable<LineData> GetChartData();
    }
}
