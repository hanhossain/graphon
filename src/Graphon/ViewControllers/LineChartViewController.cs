using System;
using System.Collections.Generic;
using System.Linq;
using Graphon.Core;
using Graphon.iOS;
using Graphon.iOS.Views;
using UIKit;

namespace Graphon.ViewControllers
{
	public class LineChartViewController : UIViewController, IChartDataSource
	{
		private readonly List<(double Angle, double Amplitude)> _sineData = new List<(double Angle, double Amplitude)>();

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

			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) =>
			{
				(var lastAngle, _) = _sineData.Last();
				var newAngle = lastAngle + Math.PI / 8;
				_sineData.Add((newAngle, Math.Sin(newAngle)));

				InvokeOnMainThread(() => lineChartView.ReloadData());
			});

			AddConstraints(lineChartView);

            // generate sine wave [0, 2π]
            for (double angle = -2 * Math.PI; angle < 2 * Math.PI; angle += Math.PI / 8)
            {
				_sineData.Add((angle, Math.Sin(angle)));
            }

			_sineData.Add((2 * Math.PI, Math.Sin(2 * Math.PI)));
        }

		public IEnumerable<LineData> GetChartData()
		{
			return new[]
			{
				new LineData()
				{
					Color = UIColor.SystemBlueColor,
					Entries = _sineData.Select(x => new ChartEntry()
					{
						X = x.Angle,
						Y = x.Amplitude
					})
				}
			};
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
