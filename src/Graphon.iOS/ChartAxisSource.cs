using System;
using System.Collections.Generic;
using System.Linq;
using Graphon.Core;

namespace Graphon.iOS
{
    public class ChartAxisSource : IChartAxisSource<double, double>
    {
        private readonly Func<IEnumerable<ChartEntry<double, double>>> _getEntries;

        private List<int> _xValues;
        private List<int> _yValues;
        private BoundsContext<double, double> _context;

        public ChartAxisSource(Func<IEnumerable<ChartEntry<double, double>>> getEntries)
        {
            _getEntries = getEntries ?? throw new ArgumentNullException(nameof(getEntries));
        }

        public double GetXAxisValue(int index)
        {
            return _xValues[index];
        }

        public double GetYAxisValue(int index)
        {
            return _yValues[index];
        }

        public BoundsContext<double, double> GetBoundsContext()
        {
            var entries = _getEntries().ToList();
            _context = new BoundsContext<double, double>()
            {
                XMin = -10,
                XMax = 10,
                YMin = -10,
                YMax = 10
            };

            if (entries.Any())
            {
                _context.XMax = Math.Ceiling(entries.Max(x => x.X));
                _context.XMin = Math.Floor(entries.Min(x => x.X));
                _context.YMax = Math.Ceiling(entries.Max(x => x.Y));
                _context.YMin = Math.Floor(entries.Min(x => x.Y));

                // always show both axes
                _context.XMin = _context.XMin > 0 ? 0 : _context.XMin;
                _context.XMax = _context.XMax < 0 ? 0 : _context.XMax;
                _context.YMin = _context.YMin > 0 ? 0 : _context.YMin;
                _context.YMax = _context.YMax < 0 ? 0 : _context.YMax;
            }

            return _context;
        }

        public (int X, int Y) GetAxisTickCount()
        {
            _xValues = new List<int>();
            _yValues = new List<int>();

            // generate the axis values
            for (int x = (int)_context.XMin; x <= (int)_context.XMax; x++)
            {
                _xValues.Add(x);
            }

            for (int y = (int)_context.YMin; y <= (int)_context.YMax; y++)
            {
                _yValues.Add(y);
            }

            return (_xValues.Count, _yValues.Count);
        }
    }
}
