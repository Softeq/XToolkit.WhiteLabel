// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.Droid;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainPageActivity : ActivityBase<MainPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }
    }
}