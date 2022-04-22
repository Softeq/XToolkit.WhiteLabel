// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Droid.Views
{
    [Activity(
        Theme = "@style/AppTheme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Dependencies.PageNavigationService
                .For<StartPageViewModel>()
                .Navigate();

            Finish();
        }
    }
}
