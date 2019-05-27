// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.App;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme.Splash")]
    [StartActivity]
    public class StartPageActivity : ActivityBase<StartPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_start);
        }
    }
}
