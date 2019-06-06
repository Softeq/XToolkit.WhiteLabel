// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Views;
using Softeq.XToolkit.WhiteLabel.Controls.Views;
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

        protected BottomNavigationView BottomNavigationView { get; private set; }

        protected abstract ColorStateList BadgeBackgroundColor { get; }

        protected abstract ColorStateList BadgeTextColor { get; }

        protected virtual void InflateMenu()
        {
            int i = 0;

            foreach(var tabViewModel in ViewModel.TabViewModels)
            {
                var iconId = GetImageResourceId(tabViewModel.ImageKey);

                BottomNavigationView.Menu
                    .Add(Menu.None, i, Menu.None, tabViewModel.Title)
                    .SetIcon(iconId);

                var itemView = BottomNavigationView.FindViewById<BottomNavigationItemView>(i++);
                var badgeView = new BadgeView(this);
                badgeView.TextColor = BadgeTextColor;
                badgeView.BackgroundColor = BadgeBackgroundColor;
                badgeView.SetViewModel(tabViewModel);
                itemView.AddView(badgeView);
            }
        }

        protected virtual int GetImageResourceId(string key)
        {
            var iconIdentifier = string.Concat("ic_", key.ToLower());
            return Resources.GetIdentifier(iconIdentifier, "drawable", PackageName);
        }

        private void BottomNavigationViewNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            var index = BottomNavigationView.GetIndex(e.Item);
            TabSelected(index);
        }
    }
}
