using System;
using Halo.Core.ViewModels.Tab;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
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

        protected abstract UITabBarItem[] TabBarItems { get; }

        private void AddTabBarView()
        {
            _tabBarController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.TabViewModels,
                TabBarItems,
                Dependencies.IocContainer.Resolve<IViewLocator>());

            _tabBarController.AddAsChildWithConstraints(this);
        }
    }
}
