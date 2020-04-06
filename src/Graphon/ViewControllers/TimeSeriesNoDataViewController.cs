using System;
using Foundation;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
    public class TimeSeriesNoDataViewController : UIViewController, IChartDataSource<DateTime, double>, IChartAxisSource<DateTime, double>
    {
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
            throw new NotImplementedException();
        }

        #endregion

        #region IChartAxisSource<DateTime, double>

        public DateTime GetXAxisValue(int axisIndex)
        {
            throw new NotImplementedException();
        }

        public double GetYAxisValue(int axisIndex)
        {
            throw new NotImplementedException();
        }

        public BoundsContext<DateTime, double> GetBoundsContext()
        {
            throw new NotImplementedException();
        }

        public (int X, int Y) GetAxisTickCount(double width, double height, double scale)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
