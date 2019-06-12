// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class ViewControllerProvider : IViewControllerProvider
    {
        public UIViewController GetRootViewController(UIViewController controller)
        {
            if (controller.PresentedViewController != null)
            {
                var presentedViewController = controller.PresentedViewController;
                return GetRootViewController(presentedViewController);
            }

            switch (controller)
            {
                case UINavigationController navigationController:
                    return GetRootViewController(navigationController.VisibleViewController);
                case UITabBarController tabBarController:
                    return GetRootViewController(tabBarController.SelectedViewController);
            }

            return controller;
        }
    }
}
