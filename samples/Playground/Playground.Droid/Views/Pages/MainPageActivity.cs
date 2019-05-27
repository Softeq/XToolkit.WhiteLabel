// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
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

            SetContentView(Resource.Layout.activity_main);

            var collectionBtn = FindViewById<Button>(Resource.Id.activity_main_collection_button);
            collectionBtn.Click += CollectionBtn_Click;
        }

        private void CollectionBtn_Click(object sender, System.EventArgs e)
        {
            ViewModel.NavigateToCollectionsSample();
        }
    }
}