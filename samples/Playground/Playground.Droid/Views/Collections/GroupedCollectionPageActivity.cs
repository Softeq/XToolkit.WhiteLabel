// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Common.Collections;
using Playground.ViewModels.Collections.Products;
using Android.Widget;

namespace Playground.Droid.Views.Collections
{
    // TODO YP:
    // - add loader
    // - add two-way
    // - add for grid layout
    // - add custom list header
    // - add custom list footer

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

            var adapter = new CustomAdapter(ViewModel.ProductListViewModel.Products)
            {
                HeaderSectionViewHolder = typeof(ProductHeaderViewHolder),
                FooterSectionViewHolder = typeof(ProductFooterViewHolder),
                ItemClick = ViewModel.AddToCartCommand
            };

            _recyclerView.SetAdapter(adapter);
        }
    }

    public class CustomAdapter : BindableGroupRecyclerViewAdapter<ProductHeaderViewModel, ProductViewModel, ProductViewHolder>
    {
        public CustomAdapter(ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> items) : base(items)
        {
        }

        // custom inflating for section footer without BindableViewHolderLayout attribute
        protected override View GetCustomLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            var textView = new TextView(parent.Context);
            var lp = new RecyclerView.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            textView.LayoutParameters = lp;

            return textView;
        }
    }
}
