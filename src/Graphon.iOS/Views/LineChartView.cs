using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Graphon.iOS;
using Graphon.iOS.Extensions;
using UIKit;

namespace Graphon.iOS.Views
{
    public class LineChartView<Tx, Ty> : UIView
        where Tx : struct
        where Ty : struct
    {
        private const int TickSize = 10;

        private readonly double _pointSize;
        private readonly IChartDataSource<Tx, Ty> _chartDataSource;
        private readonly IChartAxisSource<Tx, Ty> _chartAxisSource;

        private readonly List<LineData<Tx, Ty>> _lines = new List<LineData<Tx, Ty>>();

        private IEnumerable<IEnumerable<(ChartEntry<Tx, Ty> Entry, DataPointView View)>> _entries;
        private bool _completedInitialLoad;

        private readonly UIStringAttributes _axisStringAttributes = new UIStringAttributes()
        {
            ForegroundColor = UIColor.SystemGrayColor,
            Font = UIFont.PreferredCaption2
        };

        public LineChartView(IChartDataSource<Tx, Ty> chartDataSource, IChartAxisSource<Tx, Ty> chartAxisSource)
        {
            _chartDataSource = chartDataSource ?? throw new ArgumentNullException(nameof(chartDataSource));
            _chartAxisSource = chartAxisSource ?? throw new ArgumentNullException(nameof(chartAxisSource));
            _pointSize = 10;
        }

        public UIEdgeInsets EdgeInsets { get; set; } = new UIEdgeInsets(20, 20, 20, 20);

        public void ReloadData()
        {
            // remove existing views
            var views = _entries.SelectMany(x => x).Select(x => x.View).ToList();
            foreach (var view in views)
            {
                view.RemoveFromSuperview();
            }

            LoadData();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!_completedInitialLoad)
            {
                LoadData();
                _completedInitialLoad = true;
            }

            // redraw when the rotation changes
            SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            nfloat horizontalInset = EdgeInsets.Left + EdgeInsets.Right;
            nfloat verticalInset = EdgeInsets.Top + EdgeInsets.Bottom;
            var chartSize = new CGSize(rect.Width - horizontalInset, rect.Height - verticalInset);

            // get bounds in original coordinate system
            var bounds = _chartAxisSource.GetBoundsContext();

            // map original coordinate system to int based coordinate system
            nfloat xMin = (nfloat)_chartAxisSource.MapToXCoordinate(bounds.XMin);
            nfloat xMax = (nfloat)_chartAxisSource.MapToXCoordinate(bounds.XMax);
            nfloat yMin = (nfloat)_chartAxisSource.MapToYCoordinate(bounds.YMin);
            nfloat yMax = (nfloat)_chartAxisSource.MapToYCoordinate(bounds.YMax);
            nfloat domain = xMax - xMin;
            nfloat range = yMax - yMin;

            nfloat xCoefficient = chartSize.Width / domain;
            nfloat yCoefficient = -chartSize.Height / range;

            nfloat xDelta = (nfloat)Math.Abs(xMin) / domain * chartSize.Width + EdgeInsets.Left;
            nfloat yDelta = chartSize.Height - (nfloat)Math.Abs(yMin) / range * chartSize.Height + EdgeInsets.Top;

            var transform = new CGAffineTransform(xCoefficient, 0, 0, yCoefficient, xDelta, yDelta);

            (int xCount, int yCount) = _chartAxisSource.GetAxisTickCount();

            using var context = UIGraphics.GetCurrentContext();

            DrawXAxis(context, transform, xCount, xMin, xMax);
            DrawYAxis(context, transform, yCount, yMin, yMax);

            UIColor.SystemGrayColor.SetStroke();

            context.SetLineWidth(1);
            context.StrokePath();

            UpdateDataPoints(transform);

            DrawLines(transform);
        }

        private void LoadData()
        {
            int lineCount = _chartDataSource.NumberOfLines();
            for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
            {
                UIColor color = _chartDataSource.GetLineColor(lineIndex);
                int pointCount = _chartDataSource.NumberOfPoints(lineIndex);

                var entries = new List<ChartEntry<Tx, Ty>>();
                for (int pointIndex = 0; pointIndex < pointCount; pointIndex++)
                {
                    var indexPath = NSIndexPath.FromRowSection(pointIndex, lineIndex);
                    entries.Add(_chartDataSource.GetEntry(indexPath));
                }

                _lines.Add(new LineData<Tx, Ty>()
                {
                    Color = color,
                    Entries = entries
                });
            }

            _entries = _lines
                .Select(line => line.Entries
                    .Select(entry => (entry, new DataPointView()
                    {
                        Size = _pointSize,
                        Color = line.Color
                    }))
                    .ToList())
                .ToList();

            var views = _entries
                .SelectMany(x => x)
                .Select(x => x.View)
                .Reverse()
                .ToArray();

            AddSubviews(views);
        }

        private void UpdateDataPoints(CGAffineTransform transform)
        {
            foreach (var (entry, view) in _entries.SelectMany(x => x))
            {
                double shift = -_pointSize / 2.0;
                var calculatedPoint = transform.TransformPoint(entry.AsPoint(_chartAxisSource)).Translate(shift, shift);

                if (!view.Point.IsEqualTo(calculatedPoint))
                {
                    view.Point = calculatedPoint;
                }
            }
        }

        private void DrawXAxis(CGContext context, CGAffineTransform transform, int xCount, nfloat xMin, nfloat xMax)
        {
            context.AddLines(new[] { transform.TransformPoint(new CGPoint(xMin, 0)), transform.TransformPoint(new CGPoint(xMax, 0)) });

            for (int i = 0; i < xCount; i++)
            {
                Tx x = _chartAxisSource.GetXAxisValue(i);
                DrawXTick(x, transform, context);
                DrawXLabel(x, transform);
            }
        }

        private void DrawYAxis(CGContext context, CGAffineTransform transform, int yCount, nfloat yMin, nfloat yMax)
        {
            context.AddLines(new[] { transform.TransformPoint(new CGPoint(0, yMin)), transform.TransformPoint(new CGPoint(0, yMax)) });

            for (int i = 0; i < yCount; i++)
            {
                Ty y = _chartAxisSource.GetYAxisValue(i);
                DrawYTick(y, transform, context);
                DrawYLabel(y, transform);
            }
        }

        private void DrawXTick(Tx value, CGAffineTransform transform, CGContext context)
        {
            if (!_chartAxisSource.ShouldDrawXTick(value))
            {
                return;
            }

            // draw ticks
            var tickTopTransform = CGAffineTransform.MakeTranslation(0, -TickSize / 2);
            var tickBottomTransform = CGAffineTransform.MakeTranslation(0, TickSize / 2);

            var point = new CGPoint(_chartAxisSource.MapToXCoordinate(value), 0);
            var transformedPoint = transform.TransformPoint(point);

            var top = tickTopTransform.TransformPoint(transformedPoint);
            var bottom = tickBottomTransform.TransformPoint(transformedPoint);

            context.AddLines(new[] { top, bottom });
        }

        private void DrawYTick(Ty value, CGAffineTransform transform, CGContext context)
        {
            if (!_chartAxisSource.ShouldDrawYTick(value))
            {
                return;
            }

            // draw ticks
            var leadingTransform = CGAffineTransform.MakeTranslation(-TickSize / 2, 0);
            var trailingTransform = CGAffineTransform.MakeTranslation(TickSize / 2, 0);

            var point = new CGPoint(0, _chartAxisSource.MapToYCoordinate(value));
            var transformedPoint = transform.TransformPoint(point);

            var leading = leadingTransform.TransformPoint(transformedPoint);
            var trailing = trailingTransform.TransformPoint(transformedPoint);
            context.AddLines(new[] { leading, trailing });
        }

        private void DrawXLabel(Tx value, CGAffineTransform transform)
        {
            if (!_chartAxisSource.ShouldDrawXLabel(value))
            {
                return;
            }

            var label = (NSString)_chartAxisSource.GetXLabel(value);
            var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

            double xCoordinate = _chartAxisSource.MapToXCoordinate(value);

            var point = new CGPoint(xCoordinate, 0);
            var transformedPoint = transform.TransformPoint(point);
            var xLabelTransform = CGAffineTransform.MakeTranslation(-labelSize.Width / 2, TickSize / 2 + 3);
            var labelPoint = xLabelTransform.TransformPoint(transformedPoint);

            label.DrawString(labelPoint, _axisStringAttributes);
        }

        private void DrawYLabel(Ty value, CGAffineTransform transform)
        {
            if (!_chartAxisSource.ShouldDrawYLabel(value))
            {
                return;
            }

            var label = (NSString)_chartAxisSource.GetYLabel(value);
            var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

            double yCoordinate = _chartAxisSource.MapToYCoordinate(value);

            var point = new CGPoint(0, yCoordinate);
            var transformedPoint = transform.TransformPoint(point);
            var labelTransform = CGAffineTransform.MakeTranslation(-TickSize / 2 - labelSize.Width - 3, -labelSize.Height / 2);
            var labelPoint = labelTransform.TransformPoint(transformedPoint);

            label.DrawString(labelPoint, _axisStringAttributes);
        }

        private void DrawLines(CGAffineTransform transform)
        {
            using var context = UIGraphics.GetCurrentContext();

            // foreach (var line in _lines.Reverse())
            for (int i = _lines.Count - 1; i >= 0; i--)
            {
                var line = _lines[i];

                var points = line.Entries.Select(x => transform.TransformPoint(x.AsPoint(_chartAxisSource))).ToArray();
                context.AddLines(points);
                line.Color.SetStroke();
                context.SetLineWidth(1);
                context.StrokePath();
            }
        }
    }
}
