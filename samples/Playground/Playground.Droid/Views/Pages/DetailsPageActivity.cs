// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
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

            SetContentView(Resource.Layout.start_page);
        }
    }
}