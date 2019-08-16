// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.WeakSubscription;
using System.Collections.Specialized;

namespace Playground.Droid.Views.Collections
{
    [Activity]
    public class GroupedCollectionPageActivity : ActivityBase<GroupedCollectionPageViewModel>
    {
        private RecyclerView _recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_group_collection);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_group_collection_lst);

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            //var adapter = new BindableRecyclerViewAdapter<ProductViewModel, MovieCollectionViewHolder>(
            //    ViewModel.ProductListViewModel.Products.Values.ToList(),
            //    Resource.Layout.item_movie)
            //{
            //    ItemClick = ViewModel.AddToCartCommand
            //};

            //_recyclerView.SetAdapter(adapter);
        }
    }
}
