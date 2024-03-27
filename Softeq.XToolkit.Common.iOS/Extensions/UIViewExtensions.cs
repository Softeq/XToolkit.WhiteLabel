// Developed by Softeq Development Corporation
// http://www.softeq.com

using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:UIKit.UIView" />.
    /// </summary>
    public static class UIViewExtensions
    {
        /// <summary>
        ///     Adds view to parent subviews with parent size.
        /// </summary>
        /// <param name="view">Target view.</param>
        /// <param name="parent">Parent view.</param>
        public static void AddAsSubviewWithParentSize(this UIView view, UIView parent)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            parent.AddSubview(view);

            var right = view.RightAnchor.ConstraintEqualTo(parent.RightAnchor);
            var left = view.LeftAnchor.ConstraintEqualTo(parent.LeftAnchor);
            var top = view.TopAnchor.ConstraintEqualTo(parent.TopAnchor);
            var bottom = view.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor);

            NSLayoutConstraint.ActivateConstraints(new[] { right, left, top, bottom });
        }

        /// <summary>
        ///     UIView with the colored border.
        /// </summary>
        /// <returns>UIView with border.</returns>
        /// <param name="view">Target view.</param>
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
        /// <param name="view">Target view.</param>
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
                AddBorder(
                    view,
                    borderColor,
                    UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin,
                    new CGRect(0, 0, view.Frame.Size.Width, borderWidth));
            }

            if (edge.HasFlag(UIRectEdge.Bottom))
            {
                AddBorder(
                    view,
                    borderColor,
                    UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin,
                    new CGRect(0, view.Frame.Size.Height - borderWidth, view.Frame.Size.Width, borderWidth));
            }

            if (edge.HasFlag(UIRectEdge.Left))
            {
                AddBorder(
                    view,
                    borderColor,
                    UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleRightMargin,
                    new CGRect(0, 0, borderWidth, view.Frame.Size.Height));
            }

            if (edge.HasFlag(UIRectEdge.Right))
            {
                AddBorder(
                    view,
                    borderColor,
                    UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin,
                    new CGRect(view.Frame.Size.Width - borderWidth, 0, borderWidth, view.Frame.Size.Height));
            }

            return view;
        }

        private static UIView AddBorder(UIView view, CGColor borderColor, UIViewAutoresizing autoresizingMask, CGRect frame)
        {
            var border = new UIView
            {
                BackgroundColor = UIColor.FromCGColor(borderColor),
                AutoresizingMask = autoresizingMask,
                Frame = frame
            };

            view.AddSubview(border);

            return view;
        }

        /// <summary>
        ///     Make view corners rounded.
        /// </summary>
        /// <param name="view">Target view.</param>
        /// <param name="cornerRadius">Rounding size.</param>
        /// <returns>Rounded view.</returns>
        public static UIView WithCornerRadius(this UIView view, nfloat cornerRadius)
        {
            view.ClipsToBounds = true;

            view.Layer.CornerRadius = cornerRadius;
            view.Layer.MasksToBounds = false;

            return view;
        }

        /// <summary>
        ///     Make view rounded.
        /// </summary>
        /// <param name="view">Target view.</param>
        public static void AsCircle(this UIView view)
        {
            WithCornerRadius(view, view.Frame.Width / 2f);
        }

        /// <summary>
        ///     Use it in LayoutSubviews method.
        /// </summary>
        /// <param name="view">Target view.</param>
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

        /// <summary>
        ///     Add shadow to the view.
        /// </summary>
        /// <param name="view">Target view.</param>
        /// <param name="offset">Shadow offset.</param>
        /// <param name="color">Shadow color.</param>
        /// <param name="opacity">Shadow opacity.</param>
        /// <param name="radius">Shadow radius.</param>
        /// <param name="shadowPath">Shadow shape.</param>
        public static void WithShadow(
            this UIView view,
            CGSize offset,
            UIColor color,
            double opacity,
            double radius,
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
