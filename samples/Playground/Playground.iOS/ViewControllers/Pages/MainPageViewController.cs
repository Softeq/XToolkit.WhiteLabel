// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
    {
        private UITabBarController _tabBarController;

        public MainPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AddTabBarView();
        }

        partial void OnCollectionViewTapped(Foundation.NSObject sender)
        {
            ViewModel.NavigateToCollectionsSample();
        }

        private void AddTabBarView()
        {
            var viewLocator = Dependencies.IocContainer.Resolve<IViewLocator>();

            _tabBarController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.Dictionary,
                new[]
                {
                    new UITabBarItem(UITabBarSystemItem.Bookmarks, 0),
                    new UITabBarItem(UITabBarSystemItem.Bookmarks, 1)
                },
                viewLocator);

            _tabBarController.AddAsChildWithConstraints(this);
        }
    }
}
