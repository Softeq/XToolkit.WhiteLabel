// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomNavigation;
using Softeq.XToolkit.WhiteLabel.Droid.Controls;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using NavigationBarView = Google.Android.Material.Navigation.NavigationBarView;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public abstract class BottomNavigationComponentBase<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private readonly TViewModel _viewModel;

        protected BottomNavigationComponentBase(TViewModel viewModel, FragmentManager fragmentManager)
        {
            _viewModel = viewModel;

            var config = new FrameNavigationConfig(fragmentManager, ContainerId);
            ToolbarComponent = new ToolbarComponent<TViewModel, TKey>(config);
        }

        public BottomNavigationView? BottomNavigationView { get; private set; }

        public virtual int Layout => Resource.Layout.activity_bottom_navigation;

        public virtual ToolbarComponent<TViewModel, TKey> ToolbarComponent { get; }

        protected virtual ColorStateList? BadgeBackgroundColor { get; }

        protected virtual ColorStateList? BadgeTextColor { get; }

        protected virtual int ContainerId => Resource.Id.activity_main_page_navigation_container;

        protected virtual int BottomNavigationViewId => Resource.Id.activity_bottom_navigation_page_navigation_view;

        public virtual void Initialize(
            Func<int, BottomNavigationView?> getBottomNavigationViewFunc,
            Context context)
        {
            BottomNavigationView = getBottomNavigationViewFunc(BottomNavigationViewId);
            if (BottomNavigationView == null)
            {
                throw new ArgumentNullException(nameof(BottomNavigationView));
            }

            InflateMenu(context);
            BottomNavigationView.SelectedItemId = _viewModel.SelectedIndex;
            BottomNavigationView.ItemSelected += BottomNavigationViewNavigationItemSelected;
        }

        public virtual void Detach()
        {
        }

        protected abstract int GetImageResourceId(TKey key);

        protected virtual void HandleTabSelected(int index)
        {
            ToolbarComponent.TabSelected(_viewModel, index);
        }

        private void InflateMenu(Context context)
        {
            if (BottomNavigationView == null)
            {
                throw new ArgumentNullException(nameof(BottomNavigationView));
            }

            if (BottomNavigationView.Menu == null)
            {
                throw new ArgumentNullException(nameof(BottomNavigationView.Menu));
            }

            for (var i = 0; i < _viewModel.TabViewModels.Count; i++)
            {
                var tabViewModel = _viewModel.TabViewModels[i];
                var iconId = GetImageResourceId(tabViewModel.Key);

                var menuItem = BottomNavigationView.Menu.Add(Menu.None, i, Menu.None, tabViewModel.Title);
                menuItem?.SetIcon(iconId);

                var itemView = BottomNavigationView.FindViewById<BottomNavigationItemView>(i);
                var badgeView = new BadgeView<TKey>(context);

                if (BadgeTextColor != null)
                {
                    badgeView.TextColor = BadgeTextColor;
                }

                if (BadgeBackgroundColor != null)
                {
                    badgeView.BackgroundColor = BadgeBackgroundColor;
                }

                badgeView.SetViewModel(tabViewModel);
                itemView?.AddView(badgeView);
            }
        }

        private void BottomNavigationViewNavigationItemSelected(
            object sender,
            NavigationBarView.ItemSelectedEventArgs e)
        {
            if (BottomNavigationView == null)
            {
                throw new ArgumentNullException(nameof(BottomNavigationView));
            }

            var index = BottomNavigationView.GetIndex(e.Item);
            HandleTabSelected(index);
        }
    }
}
