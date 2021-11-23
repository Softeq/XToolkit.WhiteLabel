// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Helpers
{
    public static class UiImageHelper
    {
        public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
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

        public static UIImage FromColor(UIColor color)
        {
            var rect = new CGRect(0f, 0f, 1f, 1f);
            return FromColor(color, rect);
        }

        public static UIImage FromColor(UIColor color, CGRect rect)
        {
            UIGraphics.BeginImageContext(rect.Size);
            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(color.CGColor);
                context.FillRect(rect);
            }

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        public static UIImage MakeCircularImageWithSize(this UIImage image, CGSize size)
        {
            return image.MakeImageWithRoundedCorners(size.Width / 2, size);
        }

        public static UIImage MakeImageWithRoundedCorners(this UIImage image, nfloat cornerRadius, CGSize? size = null)
        {
            var newImageSize = size ?? image.Size;
            var circleRect = new CGRect(CGPoint.Empty, newImageSize);

            UIGraphics.BeginImageContextWithOptions(circleRect.Size, false, 0);

            var circle = UIBezierPath.FromRoundedRect(circleRect, cornerRadius);
            circle.AddClip();

            image.Draw(circleRect);

            var roundedImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return roundedImage;
        }

        public static void MakeImageViewCircular(this UIImageView imageView)
        {
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imageView.Layer.CornerRadius = imageView.Frame.Width / 2;
            imageView.Layer.MasksToBounds = false;
            imageView.ClipsToBounds = true;
        }
    }
}
