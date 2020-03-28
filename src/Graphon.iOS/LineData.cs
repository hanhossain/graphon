using System.Collections.Generic;
using Graphon.Core;
using UIKit;

namespace Graphon.iOS
{
    public class LineData
    {
        public IEnumerable<ChartEntry> Entries { get; set; }

        public UIColor Color { get; set; }
    }
}
