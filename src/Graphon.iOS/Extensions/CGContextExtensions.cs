using System;
using CoreGraphics;

namespace Graphon.iOS.Extensions
{
    public static class CGContextExtensions
    {
        public static void AddCircle(this CGContext context, nfloat x, nfloat y, nfloat radius)
        {
            context.AddArc(x, y, radius, 0, (nfloat)(2 * Math.PI), true);
        }

        public static void AddCircle(this CGContext context, double x, double y, double radius)
        {
            context.AddCircle((nfloat)x, (nfloat)y, (nfloat)radius);
        }
    }
}