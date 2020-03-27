using System;
using Foundation;
using UIKit;

namespace XamarinGraph.ViewControllers
{
	public class ChartTableViewController : UITableViewController
	{
		private const string CellId = nameof(ChartTableViewController);

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<UITableViewCell>(CellId);
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellId, indexPath);

			cell.TextLabel.Text = "Line Chart";

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var lineChartViewController = new LineChartViewController();
			NavigationController.PushViewController(lineChartViewController, true);
		}
	}
}
