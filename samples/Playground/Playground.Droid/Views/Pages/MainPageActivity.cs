// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using Softeq.XToolkit.WhiteLabel.Droid;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.Droid.Views;
using Android.Support.Design.Widget;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainPageActivity : ToolbarActivityBase<MainPageViewModel>
    {
        private BottomNavigationView _bottomNavigationView;

        protected override int NavigationContainer => Resource.Id.activity_main_page_navigation_container;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            _bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_main_page_navigation_view);
            _bottomNavigationView.NavigationItemSelected += BottomNavigationViewNavigationItemSelected;
        }

        private void BottomNavigationViewNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch(e.Item.ItemId)
            {
                case Resource.Id.main_navigation_menu_chat:
                    TabSelected(0);
                    break;
                case Resource.Id.main_navigation_menu_settimgs:
                    TabSelected(1);
                    break;
            }
        }

    }
}