namespace UIKit
{
	public static class UITableViewExtensions
	{
		public static void RegisterClassForCellReuse<T>(this UITableView tableView, string reuseIdentifier)
		{
			tableView.RegisterClassForCellReuse(typeof(T), reuseIdentifier);
		}
	}
}
