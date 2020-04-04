using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
    public class TimeSeriesNoDataViewController : UIViewController, IChartDataSource<DateTime, double>, IChartAxisSource<DateTime, double>
    {
        private readonly List<SimpleData> _data;

        private BoundsContext<DateTime, double> _boundsContext;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Line Chart (Time Series) No Data";
            View.BackgroundColor = UIColor.SystemBackgroundColor;

            var lineChartView = new LineChartView<DateTime, double>(this, this)
            {
                BackgroundColor = UIColor.SystemBackgroundColor,
                EdgeInsets = new UIEdgeInsets(30, 30, 30, 30)
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
            return 0;
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
            return _data[axisIndex].Timestamp;
        }

        public double GetYAxisValue(int axisIndex)
        {
            return _boundsContext.YMin + axisIndex;
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

        public (int X, int Y) GetAxisTickCount()
        {
            return (6, (int)(_boundsContext.YMax - _boundsContext.YMin + 1));
        }

        double IChartAxisSource<DateTime, double>.MapToXCoordinate(DateTime value)
        {
            return value.Ticks - _boundsContext.XMin.Ticks;
        }

        double IChartAxisSource<DateTime, double>.MapToYCoordinate(double value)
        {
            return value - _boundsContext.YMin;
        }

        bool IChartAxisSource<DateTime, double>.ShouldDrawXTick(DateTime value)
        {
            return true;
        }

        bool IChartAxisSource<DateTime, double>.ShouldDrawXLabel(DateTime value)
        {
            return true;
        }

        string IChartAxisSource<DateTime, double>.GetXLabel(DateTime value)
        {
            return value.ToShortTimeString();
        }

        #endregion
    }
}
