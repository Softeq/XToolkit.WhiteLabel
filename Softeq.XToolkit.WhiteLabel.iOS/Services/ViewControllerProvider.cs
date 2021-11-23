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
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                case UINavigationController navigationController:
                    return navigationController.VisibleViewController != null
                        ? GetTopViewController(navigationController.VisibleViewController)
                        : navigationController;

                case UITabBarController tabBarController:
                    return GetTopViewController(tabBarController.SelectedViewController!);

                case { } viewController
                    when viewController.ChildViewControllers.Length > 0
                         && viewController.ChildViewControllers[0]
                             is UITabBarController tabBarController:
                    return GetTopViewController(tabBarController);

                case { PresentedViewController: { } viewController }:
                    return GetTopViewController(viewController);
            }

            return controller;
        }
    }
}
