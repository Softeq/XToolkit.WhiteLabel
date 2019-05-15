// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UiViewControllerExtensions
    {
        public static void AddAsChild(this UIViewController child, UIViewController parent, UIView targetView = null)
        {
            parent.AddChildViewController(child);
            if (targetView == null)
            {
                parent.View.AddSubview(child.View);
            }
            else
            {
                targetView.AddSubview(child.View);
            }

            child.DidMoveToParentViewController(parent);
        }

        public static void SetBackButtonTitle(this UIViewController controller, string title)
        {
            controller.NavigationItem.BackBarButtonItem =
                new UIBarButtonItem(title, UIBarButtonItemStyle.Plain, null);
        }

        public static void AddAsSubviewWithParentSize(this UIView view, UIView parent)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            parent.AddSubview(view);

            var right = view.RightAnchor.ConstraintEqualTo(parent.RightAnchor);
            var left = view.LeftAnchor.ConstraintEqualTo(parent.LeftAnchor);
            var top = view.TopAnchor.ConstraintEqualTo(parent.TopAnchor);
            var bottom = view.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor);

            NSLayoutConstraint.ActivateConstraints(new[] {right, left, top, bottom});
        }

        public static void AddAsChildWithConstraints(this UIViewController child, UIViewController parent,
            UIView targetView = null)
        {
            parent.AddChildViewController(child);
            child.View.AddAsSubviewWithParentSize(targetView ?? parent.View);
            child.DidMoveToParentViewController(parent);
        }
    }
}