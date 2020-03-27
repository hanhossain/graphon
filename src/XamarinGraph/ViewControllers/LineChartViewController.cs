﻿using System;
using System.Collections.Generic;
using System.Linq;
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

			var sineData = new List<ChartEntry>();

			// generate sine wave [0, 2π]
			for (double angle = -2 * Math.PI; angle <= 2 * Math.PI; angle += Math.PI / 8)
            {
				sineData.Add(new ChartEntry()
				{
					X = angle,
					Y = Math.Sin(angle)
				});
            }

			var lineData = new LineData()
			{
				Color = UIColor.SystemBlueColor,
				Entries = sineData
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
