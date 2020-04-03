using System;
using CoreGraphics;

namespace Graphon.iOS.Extensions
{
    public static class CGPointExtensions
    {
        public static bool IsEqualTo(this CGPoint point, CGPoint targetPoint)
        {
            bool xEqual = Math.Abs(point.X - targetPoint.X) < 0.01;
            bool yEqual = Math.Abs(point.Y - targetPoint.Y) < 0.01;
            return xEqual && yEqual;
        }

        public static CGPoint Translate(this CGPoint point, double dx, double dy)
        {
            return new CGPoint(point.X + dx, point.Y + dy);
        }
    }
}