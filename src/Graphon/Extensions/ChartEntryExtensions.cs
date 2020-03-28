using CoreGraphics;
using Graphon.Core;

namespace Graphon.Extensions
{
	public static class ChartEntryExtensions
	{
		public static CGPoint AsPoint(this ChartEntry entry)
		{
			return new CGPoint(entry.X, entry.Y);
		}
	}
}
