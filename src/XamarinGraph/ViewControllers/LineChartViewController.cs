using System.Collections.Generic;
using XamarinGraph.Core;
using XamarinGraph.Graphing;
using UIKit;

namespace XamarinGraph.ViewControllers
{
	public class LineChartViewController : UIViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Line Chart";

			View.BackgroundColor = UIColor.SystemBackgroundColor;

			var lineData = new LineData()
			{
				Color = UIColor.SystemBlueColor,
				Entries = new List<ChartEntry>()
				{
					new ChartEntry() { X = 1, Y = 4 },
					new ChartEntry() { X = 3, Y = 5 }
				}
			};

			var lineChartView = new LineChartView(lineData)
			{
				BackgroundColor = UIColor.SystemBackgroundColor
			};
			View.AddSubview(lineChartView);

			lineChartView.TranslatesAutoresizingMaskIntoConstraints = false;
			lineChartView.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor).Active = true;
			lineChartView.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor).Active = true;
			lineChartView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;
			lineChartView.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;
		}
	}
}
