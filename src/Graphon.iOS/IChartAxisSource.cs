using System;

namespace Graphon.iOS
{
    public interface IChartAxisSource<Tx, Ty>
        where Tx : struct
        where Ty : struct
    {
        #region Required Methods

        Tx GetXAxisValue(int axisIndex);

        Ty GetYAxisValue(int axisIndex);

        BoundsContext<Tx, Ty> GetBoundsContext();

        (int X, int Y) GetAxisTickCount(double width, double height, double scale);

        #endregion

        #region Optional Methods

        bool ShouldDrawXTick(Tx value)
        {
            return !value.Equals(default(Tx));
        }

        bool ShouldDrawYTick(Ty value)
        {
            return !value.Equals(default(Ty));
        }

        bool ShouldDrawXLabel(Tx value)
        {
            return !value.Equals(default(Tx));
        }

        bool ShouldDrawYLabel(Ty value)
        {
            return !value.Equals(default(Ty));
        }

        double MapToXCoordinate(Tx value)
        {
            return value switch
            {
                sbyte x => x,
                byte x => x,
                short x => x,
                ushort x => x,
                int x => x,
                uint x => x,
                long x => x,
                ulong x => x,
                float x => x,
                double x => x,
                _ => throw new NotImplementedException()
            };
        }

        double MapToYCoordinate(Ty value)
        {
            return value switch
            {
                sbyte y => y,
                byte y => y,
                short y => y,
                ushort y => y,
                int y => y,
                uint y => y,
                long y => y,
                ulong y => y,
                float y => y,
                double y => y,
                _ => throw new NotImplementedException()
            };
        }

        string GetXLabel(Tx value)
        {
            return value.ToString();
        }

        string GetYLabel(Ty value)
        {
            return value.ToString();
        }

        #endregion
    }
}
