// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Helpers
{
    public static class UiTabBarControllerHelper
    {
        private static readonly IViewLocator ViewLocator;

        static UiTabBarControllerHelper()
        {
            ViewLocator = Dependencies.Container.Resolve<IViewLocator>();
        }

        public static UITabBarController CreateForViewModels(
            IEnumerable<IViewModelBase> viewModels,
            IList<UITabBarItem> tabBarItems,
            Func<UITabBarController> tabBarControllerFactory = null)
        {
            var tabBarController = tabBarControllerFactory != null
                ? tabBarControllerFactory.Invoke()
                : new UITabBarController();

            tabBarController.ViewControllers = viewModels.Select(x => ViewLocator.GetView(x)).ToArray();
            for (var i = 0; i < tabBarController.ViewControllers.Length; i++)
            {
                tabBarController.ViewControllers[i].TabBarItem = tabBarItems[i];
            }

            return tabBarController;
        }
    }
}
