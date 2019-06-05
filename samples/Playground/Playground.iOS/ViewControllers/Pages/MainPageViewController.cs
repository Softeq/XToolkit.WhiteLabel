// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class MainPageViewController : ToolbarViewControllerBase<MainPageViewModel>
    {
        public MainPageViewController(IntPtr handle) : base(handle) {  }

        protected override UITabBarItem[] TabBarItems => 
            new[] 
            {
                new UITabBarItem(UITabBarSystemItem.Bookmarks, 0),
                new UITabBarItem(UITabBarSystemItem.Bookmarks, 1)
            };
    }
}
