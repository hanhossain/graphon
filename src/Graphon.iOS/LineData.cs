using System.Collections.Generic;
using Graphon.iOS;
using UIKit;

namespace Graphon.iOS
{
    public class LineData<Tx, Ty>
        where Tx : struct
        where Ty : struct
    {
        public IEnumerable<ChartEntry<Tx, Ty>> Entries { get; set; }

        public UIColor Color { get; set; }
    }
}
