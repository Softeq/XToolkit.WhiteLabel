// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;

namespace Playground.Droid.Views
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
