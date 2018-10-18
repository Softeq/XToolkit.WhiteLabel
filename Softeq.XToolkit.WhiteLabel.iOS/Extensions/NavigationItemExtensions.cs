// Developed by Softeq Development Corporation
// http://www.softeq.com

using CoreGraphics;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Extensions
{
    public static class NavigationItemExtensions
    {
        public static void SetButton(this UINavigationItem navigationItem, UIButton button, bool left)
        {
            button.SizeToFit();
            var view = new UIView
            {
                Frame = button.Bounds
            };

            view.AddSubview(button);

            if (left)
            {
                button.Frame = new CGRect(-4, 0, button.Bounds.Width, button.Bounds.Height);
                navigationItem.SetLeftBarButtonItem(new UIBarButtonItem(view), false);
            }
            else
            {
                button.Frame = new CGRect(8, 0, button.Bounds.Width, button.Bounds.Height);
                navigationItem.SetRightBarButtonItem(new UIBarButtonItem(view), false);
            }
        }

        public static void AddTitleView(this UINavigationItem navigationItem, UIView view)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            view.LayoutIfNeeded();
            view.SizeToFit();
            view.TranslatesAutoresizingMaskIntoConstraints = true;
            navigationItem.TitleView = view;
        }
    }
}