// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Controls;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public abstract class ToolbarViewControllerBase<TViewModel, TKey> : ViewControllerBase<TViewModel>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        protected ToolbarViewControllerBase(IntPtr handle) : base(handle) { }

        protected ToolbarViewControllerBase() { }

        protected UITabBarController TabController { get; private set; }

        protected virtual UIColor BadgeColor { get; }

        protected virtual Func<UITabBarController> TabBarControllerFactory { get; } = null;

        protected abstract UIImage GetImageFromKey(TKey key);

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TabController = CreateTabBarController();
            AddTabBarView();
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
            this.Bind(() => ViewModel.SelectedIndex, OnSelectedIndexChanged);
        }

        protected virtual void AddTabBarView()
        {
            TabController.AddAsChildWithConstraints(this);
        }

        protected virtual UITabBarController CreateTabBarController()
        {
            var tabBarItems = ViewModel.TabViewModels.Select(GetTabBarItem).ToArray();

            var tabController = UiTabBarControllerHelper.CreateForViewModels(
                ViewModel.TabViewModels,
                tabBarItems,
                TryCreateRootViewController,
                TabBarControllerFactory);

            tabController.SelectedIndex = ViewModel.SelectedIndex;
            tabController.SetCommandWithArgs(
                nameof(tabController.ViewControllerSelected),
                new RelayCommand<UITabBarSelectionEventArgs>(HandleItemClick));

            return tabController;
        }

        protected virtual UITabBarItem GetTabBarItem(TabViewModel<TKey> viewModel)
        {
            var image = GetImageFromKey(viewModel.Key);
            var tabBarItem = new BindableTabBarItem<TKey>(viewModel.Title, image, image);

            if (BadgeColor != null)
            {
                tabBarItem.BadgeColor = BadgeColor;
            }

            tabBarItem.SetDataContext(viewModel);

            return tabBarItem;
        }

        protected virtual UINavigationController CreateRootViewController<TFirstViewModel>(TFirstViewModel viewModel)
            where TFirstViewModel : TabViewModel<TKey>
        {
            var rootViewController = new RootFrameNavigationControllerBase<TFirstViewModel>();

            ((IBindable) rootViewController).SetDataContext(viewModel);

            return rootViewController;
        }

        private UINavigationController TryCreateRootViewController(IViewModelBase viewModel)
        {
            if (viewModel is TabViewModel<TKey> tabViewModel)
            {
                return CreateRootViewController(tabViewModel);
            }
            throw new InvalidOperationException($"{nameof(viewModel)} must be inherited of {nameof(TabViewModel<TKey>)}");
        }

        private void HandleItemClick(UITabBarSelectionEventArgs e)
        {
            ViewModel.SelectedIndex = (int)TabController.SelectedIndex;
        }

        private void OnSelectedIndexChanged(int index)
        {
            if (TabController.ViewControllers == null || TabController.ViewControllers.Length == 0)
            {
                return;
            }

            if (TabController.SelectedIndex != index)
            {
                TabController.SelectedIndex = index;
            }
        }
    }
}