// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Linq;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public abstract class ToolbarViewControllerBase<TViewModel> : ViewControllerBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        private UITabBarController _tabBarController;

        protected ToolbarViewControllerBase(IntPtr handle) : base(handle) { }

        protected ToolbarViewControllerBase() { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AddTabBarView();
        }

        protected abstract UITabBarItem GetTabBarItem(RootFrameNavigationViewModel viewModel);

        private void AddTabBarView()
        {
            _tabBarController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.TabViewModels,
                ViewModel.TabViewModels.Select(GetTabBarItem).ToArray(),
                Dependencies.IocContainer.Resolve<IViewLocator>());

            _tabBarController.AddAsChildWithConstraints(this);
        }
    }
}
