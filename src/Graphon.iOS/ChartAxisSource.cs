using System;
using System.Collections.Generic;
using Graphon.Core;

namespace Graphon.iOS
{
    public class ChartAxisSource : IChartAxisSource<double, double>
    {
        private Func<IEnumerable<ChartEntry>> _getEntries;

        private List<int> _xValues;
        private List<int> _yValues;

        public ChartAxisSource(Func<IEnumerable<ChartEntry>> getEntries)
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

        (int X, int Y) IChartAxisSource<double, double>.GetAxisTickCount()
        {
            var chartContext = ChartContext.Create(_getEntries());

            (_xValues, _yValues) = chartContext.GenerateAxesValues();
            return (_xValues.Count, _yValues.Count);
        }
    }
}
