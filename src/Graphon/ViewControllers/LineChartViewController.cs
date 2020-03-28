using System;
using System.Collections.Generic;
using Graphon.Core;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
	public class LineChartViewController : UIViewController, IChartDataSource
	{
        public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Line Chart";

			View.BackgroundColor = UIColor.SystemBackgroundColor;

			var lineChartView = new LineChartView(this)
			{
				BackgroundColor = UIColor.SystemBackgroundColor
			};

			View.AddSubview(lineChartView);

			AddConstraints(lineChartView);
		}

		public IEnumerable<LineData> GetChartData()
		{
			var sineData = new List<ChartEntry>();

			// generate sine wave [0, 2π]
			for (double angle = -2 * Math.PI; angle < 2 * Math.PI; angle += Math.PI / 8)
			{
				sineData.Add(new ChartEntry()
				{
					X = angle,
					Y = Math.Sin(angle)
				});
			}

			sineData.Add(new ChartEntry()
			{
				X = 2 * Math.PI,
				Y = Math.Sin(2 * Math.PI)
			});

			var lineData = new LineData()
			{
				Color = UIColor.SystemBlueColor,
				Entries = sineData
			};

			return new[] { lineData };
		}

		private void AddConstraints(LineChartView lineChartView)
        {
			lineChartView.TranslatesAutoresizingMaskIntoConstraints = false;

			lineChartView.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor).Active = true;
			lineChartView.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor).Active = true;
			lineChartView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;
			lineChartView.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;
		}
	}
}
