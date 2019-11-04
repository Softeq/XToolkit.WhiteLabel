// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using CoreGraphics;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.Common.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.Helpers;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Helpers
{
    public static class AvatarImageHelpers
    {
        public static UIImage CreateAvatarWithTextPlaceholder(string name, AvatarStyles styles)
        {
            var (text, backgroundColor) = AvatarPlaceholderBuilder.Build(name, styles.BackgroundHexColors);

            return CreateCircleImage(
                styles.Size,
                backgroundColor.UIColorFromHex().CGColor,
                () => DrawText(text, styles.Size, styles.Font));
        }

        private static UIImage CreateCircleImage(Size size, CGColor backgroundColor, Action drawOnForegroundAction)
        {
            UIGraphics.BeginImageContextWithOptions(size, false, UIScreen.MainScreen.Scale);

            var context = UIGraphics.GetCurrentContext();
            var path = CGPath.EllipseFromRect(new CGRect(0, 0, size.Width, size.Height));

            context.AddPath(path);
            context.Clip();
            context.SetFillColor(backgroundColor);
            context.FillRect(new CGRect(0, 0, size.Width, size.Height));

            drawOnForegroundAction?.Invoke();

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        private static void DrawText(string text, Size size, UIFont font)
        {
            var attributedText = text.BuildAttributedString()
                .Font(font)
                .Foreground(UIColor.White);

            var textSize = attributedText.Size;
            var textPoint = new CGPoint(
                size.Width / 2f - textSize.Width / 2f,
                size.Height / 2f - textSize.Height / 2f);
            attributedText.DrawString(textPoint);
        }

        public class AvatarStyles
        {
            public AvatarStyles(Size size, UIFont font, string[] backgroundHexColors)
            {
                Size = size;
                Font = font;
                BackgroundHexColors = backgroundHexColors;
            }

            public Size Size { get; set; }
            public UIFont Font { get; set; }
            public string[] BackgroundHexColors { get; set; }
        }
    }
}
