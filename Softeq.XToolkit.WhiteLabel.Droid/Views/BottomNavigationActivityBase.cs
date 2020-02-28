// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content.Res;
using Android.OS;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Softeq.XToolkit.WhiteLabel.Droid.Controls;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class BottomNavigationActivityBase<TViewModel> : ToolbarActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_bottom_navigation);

            BottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_bottom_navigation_page_navigation_view);
            InflateMenu();
            BottomNavigationView.NavigationItemSelected += BottomNavigationViewNavigationItemSelected;
        }

        protected override int NavigationContainer => Resource.Id.activity_main_page_navigation_container;

        protected BottomNavigationView BottomNavigationView { get; private set; } = default!;

        protected virtual ColorStateList? BadgeBackgroundColor { get; }

        protected virtual ColorStateList? BadgeTextColor { get; }

        protected virtual void InflateMenu()
        {
            var i = 0;

            foreach (var tabViewModel in ViewModel.TabViewModels)
            {
                var iconId = GetImageResourceId(tabViewModel.ImageKey);

                BottomNavigationView.Menu
                    .Add(Menu.None, i, Menu.None, tabViewModel.Title)
                    .SetIcon(iconId);

                var itemView = BottomNavigationView.FindViewById<BottomNavigationItemView>(i++);
                var badgeView = new BadgeView(this);

                if (BadgeTextColor != null)
                {
                    badgeView.TextColor = BadgeTextColor;
                }

                if (BadgeBackgroundColor != null)
                {
                    badgeView.BackgroundColor = BadgeBackgroundColor;
                }

                badgeView.SetViewModel(tabViewModel);
                itemView.AddView(badgeView);
            }
        }

        protected virtual int GetImageResourceId(string key)
        {
            var iconIdentifier = string.Concat("ic_", key.ToLower());
            return Resources.GetIdentifier(iconIdentifier, "drawable", PackageName);
        }

        private void BottomNavigationViewNavigationItemSelected(object sender,
            BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            var index = BottomNavigationView.GetIndex(e.Item);
            TabSelected(index);
        }
    }
}
