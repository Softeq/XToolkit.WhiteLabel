// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class BottomNavigationActivityBase<TViewModel> : ToolbarActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase
    {
        private BottomNavigationView _bottomNavigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_bottom_navigation);

            _bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_bottom_navigation_page_navigation_view);
            InflateMenu();
            _bottomNavigationView.NavigationItemSelected += BottomNavigationViewNavigationItemSelected;
        }

        protected virtual void InflateMenu()
        {
            int i = 0;

            foreach(var tab in ViewModel.TabModels)
            {
                var iconId = GetImageResourceId(tab.ImageName);

                _bottomNavigationView.Menu
                    .Add(Menu.None, i++, Menu.None, tab.Title)
                    .SetIcon(iconId);
            }
        }

        protected virtual int GetImageResourceId(string key)
        {
            var iconIdentifier = string.Concat("ic_", key.ToLower());
            return Resources.GetIdentifier(iconIdentifier, "drawable", PackageName);
        }

        private void BottomNavigationViewNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            var index = _bottomNavigationView.GetIndex(e.Item);
            TabSelected(index);
        }
    }
}
