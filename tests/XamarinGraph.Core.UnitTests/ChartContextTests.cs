using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XamarinGraph.Core.UnitTests
{
	public class ChartContextTests
	{
		[Fact]
		public void Create_NullEntries()
		{
			Assert.Throws<ArgumentNullException>(() => ChartContext.Create(null));
		}

		[Fact]
		public void Create_NoEntries()
		{
			var context = ChartContext.Create(Enumerable.Empty<ChartEntry>());

			Assert.Equal(0, context.XMin);
			Assert.Equal(5, context.XMax);
			Assert.Equal(5, context.XRange);
			Assert.Equal(0, context.YMin);
			Assert.Equal(5, context.YMax);
			Assert.Equal(5, context.YRange);
		}

		[Fact]
		public void Create_AlwaysShowAxis_XsArePositive()
		{
			var entries = new List<ChartEntry>()
			{
				new ChartEntry() { X = 1, Y = 1 },
				new ChartEntry() { X = 9, Y = 1 }
			};

			var context = ChartContext.Create(entries);

			Assert.Equal(0, context.XMin);
			Assert.Equal(9, context.XMax);
		}

		[Fact]
		public void Create_AlwaysShowAxis_XsAreNegative()
		{
			var entries = new List<ChartEntry>()
			{
				new ChartEntry() { X = -1, Y = 1 },
				new ChartEntry() { X = -9, Y = 1 }
			};

			var context = ChartContext.Create(entries);

			Assert.Equal(-9, context.XMin);
			Assert.Equal(0, context.XMax);
		}

		[Fact]
		public void Create_AlwaysShowAxis_YsArePositive()
		{
			var entries = new List<ChartEntry>()
			{
				new ChartEntry() { X = 1, Y = 1 },
				new ChartEntry() { X = 1, Y = 9 }
			};

			var context = ChartContext.Create(entries);

			Assert.Equal(0, context.YMin);
			Assert.Equal(9, context.YMax);
		}

		[Fact]
		public void Create_AlwaysShowAxis_YsAreNegative()
		{
			var entries = new List<ChartEntry>()
			{
				new ChartEntry() { X = 1, Y = -1 },
				new ChartEntry() { X = 1, Y = -9 }
			};

            var context = ChartContext.Create(entries);

			Assert.Equal(-9, context.YMin);
			Assert.Equal(0, context.YMax);
		}

		[Fact]
		public void Create_WithDoubles()
		{
			var entries = new List<ChartEntry>()
			{
				new ChartEntry() { X = -4.5, Y = -4.5 },
				new ChartEntry() { X = 4.5, Y = 4.5 }
			};

			var context = ChartContext.Create(entries);

			Assert.Equal(5, context.XMax);
			Assert.Equal(-5, context.XMin);
			Assert.Equal(5, context.YMax);
			Assert.Equal(-5, context.YMin);
		}
	}
}
