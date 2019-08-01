// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Pages
{
    [Activity]
    public class DetailsPageActivity : ActivityBase<DetailsPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_details);
        }
    }
}