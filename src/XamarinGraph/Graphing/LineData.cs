using System.Collections.Generic;
using UIKit;
using XamarinGraph.Core;

namespace XamarinGraph.Graphing
{
	public class LineData
	{
		public IEnumerable<ChartEntry> Entries { get; set; }

		public UIColor Color { get; set; }
	}
}
