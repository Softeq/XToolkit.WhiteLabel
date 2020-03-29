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
            switch (controller)
            {
                case UINavigationController navigationController:
                    return navigationController.VisibleViewController != null
                        ? GetTopViewController(navigationController.VisibleViewController)
                        : navigationController;

                case UITabBarController tabBarController:
                    return GetTopViewController(tabBarController.SelectedViewController);

                case UIViewController viewController
                    when viewController.ChildViewControllers.Length > 0
                         && viewController.ChildViewControllers[0]
                             is UITabBarController tabBarController:
                    return GetTopViewController(tabBarController);

                case UIViewController viewController
                    when viewController.PresentedViewController
                        is UIViewController presentedViewController:
                    return GetTopViewController(presentedViewController);
            }

            return controller;
        }
    }
}
