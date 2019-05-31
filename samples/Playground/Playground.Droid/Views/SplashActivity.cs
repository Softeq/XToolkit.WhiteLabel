// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Softeq.XToolkit.WhiteLabel;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views
{
    [Activity(
        Theme = "@style/AppTheme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Dependencies.PageNavigationService
                .For<StartPageViewModel>()
                .Navigate();

            Finish();
        }
    }
}
