// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Collections
{
    [Activity(Theme = "@style/AppTheme")]
    public class CollectionPageActivity : ActivityBase<CollectionPageViewModel>
    {
        private RecyclerView? _recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_collection);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_collection_lst);

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var adapter = new BindableRecyclerViewAdapter<ItemViewModel, MovieCollectionViewHolder>(
                ViewModel.ItemModels)
            {
                ItemClick = ViewModel.SelectItemCommand
            };

            _recyclerView.SetAdapter(adapter);
        }
    }
}
