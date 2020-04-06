using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Foundation;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
    public class TimeSeriesNoZeroViewController : UIViewController, IChartDataSource<DateTime, double>, IChartAxisSource<DateTime, double>
    {
        private readonly List<SimpleData> _data = new List<SimpleData>();

        private BoundsContext<DateTime, double> _boundsContext;
        private DateTime[] _xAxisValues;
        private double[] _yAxisValues;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Line Chart (Time Series) No Zero";
            View.BackgroundColor = UIColor.SystemBackgroundColor;

            DateTime now = DateTime.Now;
            for (int i = 5; i >= 0; i--)
            {
                _data.Add(new SimpleData()
                {
                    Timestamp = now.AddMinutes(-i),
                    Value = 70 + i
                });
            }

            var lineChartView = new LineChartView<DateTime, double>(this, this)
            {
                BackgroundColor = UIColor.SystemBackgroundColor,
                EdgeInsets = new UIEdgeInsets(30, 50, 30, 30)
            };

            View.AddSubview(lineChartView);

            AddConstraints(lineChartView);
        }

        private void AddConstraints(LineChartView<DateTime, double> lineChartView)
        {
            lineChartView.TranslatesAutoresizingMaskIntoConstraints = false;

            lineChartView.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor).Active = true;
            lineChartView.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor).Active = true;
            lineChartView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;
            lineChartView.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;
        }

        #region IChartDataSource<DateTime, double>

        public int NumberOfLines()
        {
            return 1;
        }

        public int NumberOfPoints(int lineIndex)
        {
            return _data.Count;
        }

        public UIColor GetLineColor(int lineIndex)
        {
            return UIColor.SystemBlueColor;
        }

        public ChartEntry<DateTime, double> GetEntry(NSIndexPath indexPath)
        {
            var data = _data[indexPath.Row];
            return new ChartEntry<DateTime, double>()
            {
                X = data.Timestamp,
                Y = data.Value
            };
        }

        #endregion

        #region IChartAxisSource<DateTime, double>

        public DateTime GetXAxisValue(int axisIndex)
        {
            return _xAxisValues[axisIndex];
        }

        public double GetYAxisValue(int axisIndex)
        {
            return _yAxisValues[axisIndex];
        }

        public BoundsContext<DateTime, double> GetBoundsContext()
        {
            _boundsContext = new BoundsContext<DateTime, double>()
            {
                XMin = _data.First().Timestamp,
                XMax = _data.Last().Timestamp,
                YMin = _data.Min(x => x.Value),
                YMax = _data.Max(x => x.Value)
            };

            return _boundsContext;
        }

        public (int X, int Y) GetAxisTickCount(double width, double height, double scale)
        {
            int xTicks = (int)(6 * scale);
            int yTicks = 12;

            // calculate x axis values
            long deltaX = (_boundsContext.XMax.Ticks - _boundsContext.XMin.Ticks) / (xTicks - 1);
            _xAxisValues = new DateTime[xTicks];

            for (int i = 0; i < xTicks; i++)
            {
                long ticks = _boundsContext.XMin.Ticks + deltaX * i;
                _xAxisValues[i] = new DateTime(ticks);
            }

            // calculate y axis values
            double deltaY = (_boundsContext.YMax - _boundsContext.YMin) / (yTicks - 1);
            _yAxisValues = new double[yTicks];

            for (int i = 0; i < yTicks; i++)
            {
                _yAxisValues[i] = _boundsContext.YMin + deltaY * i;
            }


            return (xTicks, yTicks);
        }

        double IChartAxisSource<DateTime, double>.MapToXCoordinate(DateTime value)
        {
            return value.Ticks - _boundsContext.XMin.Ticks;
        }

        double IChartAxisSource<DateTime, double>.MapToYCoordinate(double value)
        {
            return value - _boundsContext.YMin;
        }

        string IChartAxisSource<DateTime, double>.GetXLabel(DateTime value)
        {
            return value.ToLocalTime().ToString("T", CultureInfo.CurrentCulture);
        }

        string IChartAxisSource<DateTime, double>.GetYLabel(double value)
        {
            return value.ToString("N2");
        }

        #endregion
    }
}
