// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UIViewExtensions
    {
        /// <summary>
        /// UIView with the coloured border.
        /// </summary>
        /// <returns>UIView with border</returns>
        /// <param name="view">View.</param>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderColor">Border color. UIColor.White is default color.</param>
        public static UIView WithBorder(this UIView view, nfloat borderWidth, CGColor borderColor = null)
        {
            view.Layer.BorderWidth = borderWidth;
            view.Layer.BorderColor = borderColor ?? UIColor.White.CGColor;

            return view;
        }

        public static UIView WithCornerRadius(this UIView view, nfloat cornerRadius)
        {
            view.ClipsToBounds = true;

            view.Layer.CornerRadius = cornerRadius;
            view.Layer.MasksToBounds = false;

            return view;
        }

        public static void AsCircle(this UIView view)
        {
            WithCornerRadius(view, view.Frame.Width / 2f);
        }

        public static void WithCornerRadius(this UIView view, UIRectCorner corners, float radius)
        {
            view.ClipsToBounds = true;
            var path = UIBezierPath.FromRoundedRect(view.Bounds, corners, new CGSize(radius, radius));
            var maskLayer = new CAShapeLayer
            {
                Path = path.CGPath
            };
            view.Layer.Mask = maskLayer;
        }
    }
}
