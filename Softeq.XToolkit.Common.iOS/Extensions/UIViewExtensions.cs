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
        public static UIView WithBorder(this UIView view, nfloat borderWidth, CGColor? borderColor = null)
        {
            view.Layer.BorderWidth = borderWidth;
            view.Layer.BorderColor = borderColor ?? UIColor.White.CGColor;

            return view;
        }

        /// <summary>
        ///     UIView with the colored border for target edges.
        /// </summary>
        /// <returns>UIView with border.</returns>
        /// <param name="view">View.</param>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="edge">Border edge.</param>
        public static UIView WithBorder(this UIView view, nfloat borderWidth, CGColor borderColor, UIRectEdge edge)
        {
            if (edge == UIRectEdge.None)
            {
                return view;
            }

            if (edge.HasFlag(UIRectEdge.All))
            {
                return WithBorder(view, borderWidth, borderColor);
            }

            if (edge.HasFlag(UIRectEdge.Top))
            {
                AddBorder(view, borderWidth, borderColor, UIRectEdge.Top);
            }

            if (edge.HasFlag(UIRectEdge.Bottom))
            {
                AddBorder(view, borderWidth, borderColor, UIRectEdge.Bottom);
            }

            if (edge.HasFlag(UIRectEdge.Left))
            {
                AddBorder(view, borderWidth, borderColor, UIRectEdge.Left);
            }

            if (edge.HasFlag(UIRectEdge.Right))
            {
                AddBorder(view, borderWidth, borderColor, UIRectEdge.Right);
            }

            return view;
        }

        private static UIView AddBorder(UIView view, nfloat borderWidth, CGColor borderColor, UIRectEdge edge)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            var autoresizingMask = edge switch
            {
                UIRectEdge.Top => UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin,
                UIRectEdge.Bottom => UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin,
                UIRectEdge.Left => UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleRightMargin,
                UIRectEdge.Right => UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin,
            };

            var frame = edge switch
            {
                UIRectEdge.Top => new CGRect(0, 0, view.Frame.Size.Width, borderWidth),
                UIRectEdge.Bottom => new CGRect(0, view.Frame.Size.Height - borderWidth, view.Frame.Size.Width, borderWidth),
                UIRectEdge.Left => new CGRect(0, 0, borderWidth, view.Frame.Size.Height),
                UIRectEdge.Right => new CGRect(view.Frame.Size.Width - borderWidth, 0, borderWidth, view.Frame.Size.Height),
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

            var border = new UIView
            {
                BackgroundColor = UIColor.FromCGColor(borderColor),
                AutoresizingMask = autoresizingMask,
                Frame = frame
            };

            view.AddSubview(border);

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
            UIBezierPath? shadowPath = null)
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
