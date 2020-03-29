// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Graphics;
using Android.OS;
using Playground.ViewModels.BottomTabs;
using Softeq.XToolkit.WhiteLabel.Droid.Views;

namespace Playground.Droid.Views.BottomTabs
{
    [Activity(Theme = "@style/AppTheme")]
    public class BottomTabsPageActivity : BottomNavigationActivityBase<BottomTabsPageViewModel, string>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BottomNavigationView.SetBackgroundColor(Color.White);
        }

        protected override int GetImageResourceId(string key)
        {
            var iconIdentifier = string.Concat("ic_", key.ToLower());
            return Resources.GetIdentifier(iconIdentifier, "drawable", PackageName);
        }
    }
}