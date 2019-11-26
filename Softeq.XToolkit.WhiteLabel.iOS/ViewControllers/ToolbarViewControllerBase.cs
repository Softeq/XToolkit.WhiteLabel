// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Controls;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public abstract class ToolbarViewControllerBase<TViewModel> : ViewControllerBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        protected ToolbarViewControllerBase(IntPtr handle) : base(handle) { }

        protected ToolbarViewControllerBase() { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AddTabBarView();
        }

        protected UITabBarController TabController { get; private set; }

        protected virtual UIColor BadgeColor { get; }

        protected virtual Func<UITabBarController> TabBarControllerFactory { get; } = null;

        protected virtual void AddTabBarView()
        {
            var tabBarItems = ViewModel.TabViewModels.Select(GetTabBarItem).ToArray();

            TabController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.TabViewModels,
                tabBarItems,
                CreateRootViewController,
                TabBarControllerFactory);

            TabController.AddAsChildWithConstraints(this);
        }

        protected virtual UITabBarItem GetTabBarItem(TabViewModel viewModel)
        {
            var image = GetImageFromKey(viewModel.ImageKey);
            var tabBarItem = new BindableTabBarItem(viewModel.Title, image, image);

            if (BadgeColor != null)
            {
                tabBarItem.BadgeColor = BadgeColor;
            }

            tabBarItem.SetDataContext(viewModel);

            return tabBarItem;
        }

        protected virtual UIImage GetImageFromKey(string key)
        {
            return UIImage.FromBundle(string.Concat("ic", key));
        }

        protected virtual UINavigationController CreateRootViewController(IViewModelBase viewModel)
        {
            if (viewModel is TabViewModel tabViewModel)
            {
                return CreateRootViewController(tabViewModel);
            }
            throw new InvalidOperationException($"{nameof(viewModel)} must be inherited of {nameof(TabViewModel)}");
        }

        protected virtual UINavigationController CreateRootViewController<TFirstViewModel>(TFirstViewModel viewModel)
            where TFirstViewModel : TabViewModel
        {
            var rootViewController = new RootFrameNavigationControllerBase<TFirstViewModel>();

            ((IBindable) rootViewController).SetDataContext(viewModel);

            ConfigureRootViewController(rootViewController);

            return rootViewController;
        }

        protected virtual void ConfigureRootViewController(UINavigationController rootViewController)
        {
            rootViewController.NavigationBarHidden = true;
        }
    }
}
