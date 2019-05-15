// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UIImageExtensions
    {
        public static UIImage MakeCircularImageWithSize(this UIImage image, CGSize size)
        {
            var circleRect = new CGRect(CGPoint.Empty, size);

            UIGraphics.BeginImageContextWithOptions(circleRect.Size, false, 0);

            var circle = UIBezierPath.FromRoundedRect(circleRect, circleRect.Size.Width / 2);
            circle.AddClip();

            image.Draw(circleRect);

            var roundedImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return roundedImage;
        }

        public static UIImage MaxResizeImage(this UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1)
            {
                return sourceImage;
            }

            var width = (float)(maxResizeFactor * sourceSize.Width);
            var height = (float)(maxResizeFactor * sourceSize.Height);

            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return resultImage;
        }

        public static UIImage ToUpImageOrientation(this UIImage image)
        {
            var orientation = image.Orientation;

            if (orientation == UIImageOrientation.Up)
            {
                return image;
            }

            int degree = 0;
            switch (orientation)
            {
                case UIImageOrientation.Down:
                    degree = 180;
                    break;
                case UIImageOrientation.Left:
                    degree = 270;
                    break;
                case UIImageOrientation.Right:
                    degree = 90;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rect = orientation == UIImageOrientation.Down
                ? new CGRect(0, 0, image.Size.Width, image.Size.Height)
                : new CGRect(0, 0, image.Size.Height, image.Size.Width);
            var view = new UIView(rect);

            var radians = degree * (float)Math.PI / 180;
            var t = CGAffineTransform.MakeRotation(radians);
            view.Transform = t;

            CGSize size = view.Frame.Size;
            UIGraphics.BeginImageContext(size);
            CGContext context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(radians);
            context.ScaleCTM(1, -1);
            context.DrawImage(new CGRect(-rect.Width / 2, -rect.Height / 2, rect.Width, rect.Height), image.CGImage);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
    }
}