// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class MainPageViewController : ToolbarViewControllerBase<MainPageViewModel>
    {
        public MainPageViewController(IntPtr handle) : base(handle) {  }

        protected override UITabBarItem GetTabBarItem(RootFrameNavigationViewModel viewModel)
        {
            switch(ViewModel.TabViewModels.IndexOf(viewModel))
            {
                case 0:
                    return new UITabBarItem(UITabBarSystemItem.Bookmarks, 0);
                case 1:
                    return new UITabBarItem(UITabBarSystemItem.Contacts, 1);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
