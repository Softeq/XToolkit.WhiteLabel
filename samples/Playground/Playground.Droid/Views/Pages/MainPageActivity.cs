// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.Droid.Views;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainPageActivity : BottomNavigationActivityBase<MainPageViewModel>
    {
        protected override int NavigationContainer => Resource.Id.activity_main_page_navigation_container;

        protected override int MenuId => Resource.Menu.main_navigation_menu;
    }
}