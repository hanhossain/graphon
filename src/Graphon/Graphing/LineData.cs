using System.Collections.Generic;
using Graphon.Core;
using UIKit;

namespace Graphon.Graphing
{
	public class LineData
	{
		public IEnumerable<ChartEntry> Entries { get; set; }

		public UIColor Color { get; set; }
	}
}
