// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.Droid;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views.Pages
{
    [Activity]
    public class DetailsPageActivity : ActivityBase<DetailsPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.start_page);
        }
    }
}