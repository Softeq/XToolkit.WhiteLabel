// Developed by Softeq Development Corporation
// http://www.softeq.com

 using System;
using System.Linq;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS.Controls;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public class ToolbarViewControllerBase<TViewModel> : ViewControllerBase<TViewModel>
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

        protected virtual UIColor BadgeColor { get; }

        protected virtual UITabBarItem GetTabBarItem(RootFrameNavigationViewModel viewModel)
        {
            var image = GetImageFromKey(viewModel.ImageKey);
            var tabBarItem = new BindableTabBarItem(viewModel.Title, image, image);
            if(BadgeColor != null)
            {
                tabBarItem.BadgeColor = BadgeColor;
            }
            tabBarItem.SetViewModel(viewModel);
            return tabBarItem;
        }

        protected virtual UIImage GetImageFromKey(string key)
        {
            return UIImage.FromBundle(string.Concat("ic", key));
        }

        private void AddTabBarView()
        {
            _tabBarController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.TabViewModels,
                ViewModel.TabViewModels.Select(GetTabBarItem).ToArray(),
                Dependencies.Container.Resolve<IViewLocator>());

            _tabBarController.AddAsChildWithConstraints(this);
        }
    }
}
