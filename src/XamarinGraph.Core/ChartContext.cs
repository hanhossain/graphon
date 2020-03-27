using System;
using System.Collections.Generic;
using System.Linq;

namespace XamarinGraph.Core
{
	public class ChartContext
	{
		private ChartContext()
		{
		}

		public int XMin { get; private set; }

		public int XMax { get; private set; }

		public int Domain => XMax - XMin;

		public int YMin { get; private set; }

		public int YMax { get; private set; }

		public int Range => YMax - YMin;

		public static ChartContext Create(IEnumerable<ChartEntry> entries)
		{
			entries = entries ?? throw new ArgumentNullException(nameof(entries));

			var context = new ChartContext()
			{
				XMax = 5,
				XMin = 0,
				YMax = 5,
				YMin = 0
			};

			var chartEntries = entries.ToList();
			if (chartEntries.Any())
			{
				context.XMax = (int)Math.Ceiling(chartEntries.Max(x => x.X));
				context.XMin = (int)Math.Floor(chartEntries.Min(x => x.X));
				context.YMax = (int)Math.Ceiling(chartEntries.Max(x => x.Y));
				context.YMin = (int)Math.Floor(chartEntries.Min(x => x.Y));

				// always show both axes
				context.XMin = context.XMin > 0 ? 0 : context.XMin;
				context.XMax = context.XMax < 0 ? 0 : context.XMax;
				context.YMin = context.YMin > 0 ? 0 : context.YMin;
				context.YMax = context.YMax < 0 ? 0 : context.YMax;
			}

			return context;
		}

		public (List<int> xValues, List<int> yValues) GenerateAxesValues(int xStep = 1, int yStep = 1)
		{
			var xValues = new List<int>();
			var yValues = new List<int>();

			// generate the axis values
			for (int x = XMin; x <= XMax; x += xStep)
			{
				xValues.Add(x);
			}

			for (int y = YMin; y <= YMax; y += yStep)
			{
				yValues.Add(y);
			}

			return (xValues, yValues);
		}
	}
}
