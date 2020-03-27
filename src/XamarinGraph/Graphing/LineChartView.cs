using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using XamarinGraph.Core;
using XamarinGraph.Extensions;
using UIKit;

namespace XamarinGraph.Graphing
{
	public class LineChartView : UIView
	{
		private const int AxisOffset = 20;
		private const int EdgeOffset = 20;
		private const int TickSize = 10;

		private readonly IEnumerable<LineData> _lines;
		private readonly IEnumerable<IEnumerable<(ChartEntry Entry, DataPointView View)>> _entries;
		private readonly ChartContext _chartContext;

		private readonly double _pointSize;

		private readonly UIStringAttributes _axisStringAttributes = new UIStringAttributes()
		{
			ForegroundColor = UIColor.SystemGrayColor,
			Font = UIFont.PreferredCaption2
		};

		public LineChartView(params LineData[] lines)
			: this(10, lines)
		{
		}

		public LineChartView(double pointSize, params LineData[] lines)
		{
			_pointSize = pointSize;
			_lines = lines ?? throw new ArgumentNullException(nameof(lines));
			_entries = lines
				.Select(line => line.Entries
					.Select(entry => (entry, new DataPointView()
					{
						Size = _pointSize,
						Color = line.Color
					}))
					.ToList())
				.ToList();

			var chartEntries = _entries.SelectMany(x => x).Select(x => x.Entry).ToList();
			_chartContext = ChartContext.Create(chartEntries);

			var views = _entries.SelectMany(x => x.Select(y => y.View)).Reverse().ToArray();
			AddSubviews(views);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			SetNeedsDisplay();
		}

		public override void Draw(CGRect rect)
		{
			var chartSize = new CGSize(rect.Width - EdgeOffset * 2, rect.Height - EdgeOffset * 2);

			nfloat xCoefficient = chartSize.Width / _chartContext.Domain;
			nfloat yCoefficient = -chartSize.Height / _chartContext.Range;
			nfloat xDelta = Math.Abs(_chartContext.XMin) / (nfloat)_chartContext.Domain * chartSize.Width + EdgeOffset;
			nfloat yDelta = chartSize.Height - Math.Abs(_chartContext.YMin) / (nfloat)_chartContext.Range * chartSize.Height + EdgeOffset;

			var transform = new CGAffineTransform(xCoefficient, 0, 0, yCoefficient, xDelta, yDelta);

			var (xValues, yValues) = _chartContext.GenerateAxesValues();

            DrawXAxis(xValues, transform);
            DrawYAxis(yValues, transform);

            //DrawXLabels(xValues, transform);
            //DrawYLabels(yValues, transform);

            UpdateDataPoints(transform);

            DrawLines(transform);
        }

		private void UpdateDataPoints(CGAffineTransform transform)
		{
			foreach (var (entry, view) in _entries.SelectMany(x => x))
			{
				double shift = -_pointSize / 2.0;
				var calculatedPoint = transform.TransformPoint(entry.AsPoint()).Translate(shift, shift);

				if (!view.Point.IsEqualTo(calculatedPoint))
				{
					view.Point = calculatedPoint;
				}
			}
		}

		private void DrawXAxis(IEnumerable<int> values, CGAffineTransform transform)
		{
			using var context = UIGraphics.GetCurrentContext();

			// draw x-axis
			context.AddLines(new[] { transform.TransformPoint(new CGPoint(_chartContext.XMin, 0)), transform.TransformPoint(new CGPoint(_chartContext.XMax, 0)) });

            // draw ticks
            var tickTopTransform = CGAffineTransform.MakeTranslation(0, -TickSize / 2);
            var tickBottomTransform = CGAffineTransform.MakeTranslation(0, TickSize / 2);

            foreach (int x in values)
            {
                // we don't need a tick at the y axis since we have that drawn
                if (x == 0)
                {
                    continue;
                }

                var point = new CGPoint(x, 0);
                var transformedPoint = transform.TransformPoint(point);

                var top = tickTopTransform.TransformPoint(transformedPoint);
                var bottom = tickBottomTransform.TransformPoint(transformedPoint);

                context.AddLines(new[] { top, bottom });
            }

            UIColor.SystemGrayColor.SetStroke();

			context.SetLineWidth(1);
			context.StrokePath();
		}

		private void DrawYAxis(IEnumerable<int> values, CGAffineTransform transform)
		{
			using var context = UIGraphics.GetCurrentContext();

			// draw y-axis
			context.AddLines(new[] { transform.TransformPoint(new CGPoint(0, _chartContext.YMin)), transform.TransformPoint(new CGPoint(0, _chartContext.YMax)) });

            // draw ticks
            var leadingTransform = CGAffineTransform.MakeTranslation(-TickSize / 2, 0);
            var trailingTransform = CGAffineTransform.MakeTranslation(TickSize / 2, 0);

            foreach (int y in values)
            {
                // we don't need a tick at the x axis since we have that drawn
                if (y == 0)
                {
                    continue;
                }

                var point = new CGPoint(0, y);
                var transformedPoint = transform.TransformPoint(point);

                var leading = leadingTransform.TransformPoint(transformedPoint);
                var trailing = trailingTransform.TransformPoint(transformedPoint);
                context.AddLines(new[] { leading, trailing });
            }

            UIColor.SystemGrayColor.SetStroke();

			context.SetLineWidth(1);
			context.StrokePath();
		}

		private void DrawXLabels(IEnumerable<int> values, CGAffineTransform transform)
		{
			foreach (int x in values)
			{
				// don't draw the 0 label
				if (x == 0)
				{
					continue;
				}

				var label = (NSString)x.ToString();
				var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

				var point = new CGPoint(x, 0);
				var transformedPoint = transform.TransformPoint(point);
				var xLabelTransform = CGAffineTransform.MakeTranslation(-labelSize.Width / 2, TickSize / 2 + 3);
				var labelPoint = xLabelTransform.TransformPoint(transformedPoint);

				label.DrawString(labelPoint, _axisStringAttributes);
			}
		}

		private void DrawYLabels(IEnumerable<int> values, CGAffineTransform transform)
		{
			foreach (int y in values)
			{
				// don't draw the 0 label
				if (y == 0)
				{
					continue;
				}

				var label = (NSString)y.ToString();
				var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

				var point = new CGPoint(0, y);
				var transformedPoint = transform.TransformPoint(point);
				var labelTransform = CGAffineTransform.MakeTranslation(-TickSize / 2 - labelSize.Width - 3, -labelSize.Height / 2);
				var labelPoint = labelTransform.TransformPoint(transformedPoint);

				label.DrawString(labelPoint, _axisStringAttributes);
			}
		}

		private void DrawLines(CGAffineTransform transform)
		{
			using var context = UIGraphics.GetCurrentContext();

			foreach (var line in _lines.Reverse())
			{
				var points = line.Entries.Select(x => transform.TransformPoint(x.AsPoint())).ToArray();
				context.AddLines(points);
				line.Color.SetStroke();
				context.SetLineWidth(1);
				context.StrokePath();
			}
		}
	}
}
