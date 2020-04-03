using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Graphon.Core;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
    public class TimeSeriesViewController : UIViewController, IChartDataSource<DateTime, double>, IChartAxisSource<DateTime, double>
    {
        private readonly List<SimpleData> _data = new List<SimpleData>();
        
        private BoundsContext<DateTime, double> _boundsContext;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Line Chart (Time Series)";
            View.BackgroundColor = UIColor.SystemBackgroundColor;

            DateTime now = DateTime.Now;
            for (int i = 5; i >= 0; i--)
            {
                _data.Add(new SimpleData()
                {
                    Timestamp = now.AddMinutes(-i),
                    Value = i * i
                });
            }
            
            var lineChartView = new LineChartView<DateTime, double>(this, this)
            {
                BackgroundColor = UIColor.SystemBackgroundColor
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
        
        public DateTime GetXAxisValue(int index)
        {
            return _data[index].Timestamp;
        }

        public double GetYAxisValue(int index)
        {
            return index;
        }

        public BoundsContext<DateTime, double> GetBoundsContext()
        {
            _boundsContext =  new BoundsContext<DateTime, double>()
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

        bool IChartAxisSource<DateTime, double>.ShouldDrawXTick(DateTime value)
        {
            return value != _boundsContext.XMin;
        }

        string IChartAxisSource<DateTime, double>.GetXLabel(DateTime value)
        {
            return value.ToShortTimeString();
        }

        #endregion
    }

    public class SimpleData
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
