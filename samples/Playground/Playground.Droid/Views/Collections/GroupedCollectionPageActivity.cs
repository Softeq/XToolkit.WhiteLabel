// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Collections
{
    [Activity]
    public class GroupedCollectionPageActivity : ActivityBase<GroupedCollectionPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_collection);
        }
    }
}
