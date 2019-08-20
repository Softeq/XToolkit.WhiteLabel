﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.WhiteLabel.Droid;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Playground.Droid.Converters;

namespace Playground.Droid.Views.Collections
{
    [Activity]
    public class GroupedCollectionPageActivity : ActivityBase<GroupedCollectionPageViewModel>
    {
        private const int ColumnsCount = 3;

        private ProgressBar _progress;
        private RecyclerView _recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_group_collection);

            _progress = FindViewById<ProgressBar>(Resource.Id.activity_group_collection_progress);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_group_collection_lst);

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            // init adapter
            var adapter = new CustomAdapter(ViewModel.ProductListViewModel.Products)
            {
                FooterViewHolder = typeof(ProductsListHeaderViewHolder),
                HeaderSectionViewHolder = typeof(ProductHeaderViewHolder),
                FooterSectionViewHolder = typeof(ProductFooterViewHolder),
                ItemClick = ViewModel.AddToCartCommand
            };
            _recyclerView.SetAdapter(adapter);

            // init layout
            var layoutManager = new GridLayoutManager(this, ColumnsCount);
            layoutManager.SetSpanSizeLookup(new GroupedSpanSizeLookup(adapter, ColumnsCount));
            _recyclerView.SetLayoutManager(layoutManager);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ProductBasketViewModel.Status, () => SupportActionBar.Title);
            this.Bind(() => ViewModel.ProductListViewModel.IsBusy, () => _progress.Visibility, new BoolToVisibilityConverter());
        }
    }

    /// <summary>
    ///     Sample of custom size for GridLayoutManager cells.
    /// </summary>
    internal class GroupedSpanSizeLookup : GridLayoutManager.SpanSizeLookup
    {
        private readonly RecyclerView.Adapter _adapter;
        private readonly int _spansCount;

        public GroupedSpanSizeLookup(RecyclerView.Adapter adapter, int spansCount)
        {
            _adapter = adapter;
            _spansCount = spansCount;
        }
        public override int GetSpanSize(int position)
        {
            var itemViewType = (ItemType) _adapter.GetItemViewType(position);

            switch (itemViewType)
            {
                case ItemType.Item:
                    return 1;
                default:
                    return _spansCount; // for headers, footers and etc.
            }
        }
    }

    /// <summary>
    ///     Sample of custom <see cref="T:BindableGroupRecyclerViewAdapter"/>
    /// </summary>
    internal class CustomAdapter : BindableGroupRecyclerViewAdapter<
        ProductHeaderViewModel, // header data type
        ProductViewModel,       // item data type
        ProductViewHolder>      // item ViewHolder type
    {
        public CustomAdapter(ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> items) : base(items)
        {
        }

        // custom inflating for ViewHolders without BindableViewHolderLayout attribute
        protected override View GetCustomLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            var textView = new TextView(parent.Context);
            var lp = new RecyclerView.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            textView.LayoutParameters = lp;

            return textView;
        }
    }
}
