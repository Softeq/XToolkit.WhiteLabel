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
        ///     UIView with the coloured border.
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

        /// <summary>
        ///     Use it in LayoutSubviews method
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="corners">Corners.</param>
        /// <param name="radius">Radius.</param>
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

        public static void WithShadow(this UIView view, CGSize offset, UIColor color, double opacity, double radius,
            UIBezierPath shadowPath = null)
        {
            view.Layer.MasksToBounds = false;
            view.Layer.ShadowColor = color.CGColor;
            view.Layer.ShadowOffset = offset;
            view.Layer.ShadowOpacity = (float) opacity;
            view.Layer.ShadowRadius = (float) radius;

            if (shadowPath != null)
            {
                view.Layer.ShadowPath = shadowPath.CGPath;
            }
        }
    }
}
