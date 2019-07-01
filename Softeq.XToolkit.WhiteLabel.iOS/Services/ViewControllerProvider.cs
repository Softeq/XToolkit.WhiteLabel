// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class ViewControllerProvider : IViewControllerProvider
    {
        public UIViewController GetTopViewController(UIViewController controller)
        {
            if (controller.PresentedViewController != null)
            {
                var presentedViewController = controller.PresentedViewController;
                return GetTopViewController(presentedViewController);
            }

            switch (controller)
            {
                case UINavigationController navigationController:
                    return GetTopViewController(navigationController.VisibleViewController);
                case UITabBarController tabBarController:
                    return GetTopViewController(tabBarController.SelectedViewController);
                case UIViewController viewController when controller.ChildViewControllers.Length > 0:
                    return GetTopViewController(viewController.ChildViewControllers[0]);
            }

            return controller;
        }
    }
}
