// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public static class UiTabBarControllerHelper
    {
        public static UITabBarController CreateForViewModels(
            IEnumerable<RootFrameNavigationViewModelBase> viewModels,
            IList<UITabBarItem> tabBarItems,
            IViewLocator viewLocator,
            Func<UITabBarController> tabBarControllerFactory = null)
        {
            var tabBarController = tabBarControllerFactory != null
                ? tabBarControllerFactory.Invoke()
                : new UITabBarController();

            tabBarController.ViewControllers = viewModels.Select(x => Create(x, viewLocator)).ToArray();
            for (var i = 0; i < tabBarController.ViewControllers.Length; i++)
            {
                tabBarController.ViewControllers[i].TabBarItem = tabBarItems[i];
            }

            return tabBarController;
        }

        private static UIViewController Create(RootFrameNavigationViewModelBase viewModel, IViewLocator viewLocator)
        {
            var root = viewLocator.GetView(viewModel);
            return root;
        }
    }
}