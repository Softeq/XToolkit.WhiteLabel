// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:UIKit.UIViewController" />.
    /// </summary>
    public static class UIViewControllerExtensions
    {
        /// <summary>
        ///     Adds the specified view controller as a child.
        /// </summary>
        /// <param name="child">Target child view controller.</param>
        /// <param name="parent">Parent view controller to add child.</param>
        /// <param name="parentView">Custom target view of parent view controller. By default <c>parent.View</c>.</param>
        public static void AddAsChild(this UIViewController child, UIViewController parent, UIView? parentView = null)
        {
            parent.AddChildViewController(child);
            (parentView ?? parent.View).AddSubview(child.View);
            child.DidMoveToParentViewController(parent);
        }

        /// <summary>
        ///      Adds the specified view controller as a child to parent with add constraints to view.
        /// </summary>
        /// <param name="child">Target child view controller.</param>
        /// <param name="parent">Parent view controller to add child.</param>
        /// <param name="parentView">Custom target view of parent view controller. By default <c>parent.View</c>.</param>
        public static void AddAsChildWithConstraints(
            this UIViewController child,
            UIViewController parent,
            UIView? parentView = null)
        {
            parent.AddChildViewController(child);
            child.View.AddAsSubviewWithParentSize(parentView ?? parent.View);
            child.DidMoveToParentViewController(parent);
        }

        /// <summary>
        ///     Removes the view controller from its parent.
        /// </summary>
        /// <param name="child">Target child ViewController.</param>
        /// <param name="parent">Parent ViewController to remove child.</param>
        public static void RemoveFromParent(this UIViewController child, UIViewController parent)
        {
            child.WillMoveToParentViewController(parent);
            child.View.RemoveFromSuperview();
            child.RemoveFromParentViewController();
        }

        /// <summary>
        ///    Sets new title to the back button on NavigationBar.
        /// </summary>
        /// <param name="controller">Target ViewController.</param>
        /// <param name="title">New back button text.</param>
        public static void SetBackButtonTitle(this UIViewController controller, string title)
        {
            var newBackButton = new UIBarButtonItem(title, UIBarButtonItemStyle.Plain, null);
            controller.NavigationItem.BackBarButtonItem = newBackButton;
        }
    }
}
