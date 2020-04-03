using System;
using Foundation;
using UIKit;

namespace Graphon.ViewControllers
{
    public class ChartTableViewController : UITableViewController
    {
        private const string CellId = nameof(ChartTableViewController);

        private readonly string[] _charts = { "Line Chart", "Line Chart (Time Series)" };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RegisterClassForCellReuse<UITableViewCell>(CellId);
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _charts.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellId, indexPath);

            cell.TextLabel.Text = _charts[indexPath.Row];

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UIViewController viewController = indexPath.Row switch
            {
                0 => new LineChartViewController(),
                1 => new TimeSeriesViewController(),
                _ => throw new NotImplementedException()
            };
            
            NavigationController.PushViewController(viewController, true);
        }
    }
}
