using CoreGraphics;
using Graphon.iOS.Extensions;
using UIKit;

namespace Graphon.iOS.Views
{
    public class DataPointView : UIView
    {
        public DataPointView()
        {
            BackgroundColor = UIColor.Clear;
        }

        public CGPoint Point
        {
            get => Frame.Location;
            set
            {
                Frame = new CGRect(value, Frame.Size);
                SetNeedsDisplay();
            }
        }

        public UIColor Color { get; set; } = UIColor.SystemRedColor;

        public double Size
        {
            get => Frame.Height;
            set
            {
                Frame = new CGRect(Frame.Location, new CGSize(value, value));
                SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            using var context = UIGraphics.GetCurrentContext();

            context.AddCircle(rect.GetMidX(), rect.GetMidY(), rect.Height / 2);
            context.SetLineWidth(0);
            context.SetFillColor(Color.CGColor);
            context.DrawPath(CGPathDrawingMode.Fill);
        }
    }
}
