﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Droid.Converters;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Collections
{
    [Activity]
    public class GroupedTablePageActivity : ActivityBase<GroupedCollectionPageViewModel>
    {
        private const int ColumnsCount = 3;

        private ProgressBar _progress = null!;
        private RecyclerView _recyclerView = null!;
        private Button _generateButton = null!;
        private Button _addButton = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_group_collection);

            _generateButton = FindViewById<Button>(Resource.Id.button_generate)!;
            _addButton = FindViewById<Button>(Resource.Id.button_add)!;
            _progress = FindViewById<ProgressBar>(Resource.Id.activity_group_collection_progress)!;
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_group_collection_lst)!;

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            // init adapter
            var adapter = new CustomAdapter(ViewModel.ProductListViewModel.Products, typeof(ProductHeaderViewHolder), typeof(ProductFooterViewHolder))
            {
                // ItemClick = ViewModel.AddToCartCommand
            };
            _recyclerView.SetAdapter(adapter);

            // init layout
            var layoutManager = new GridLayoutManager(this, ColumnsCount);
            layoutManager.SetSpanSizeLookup(new GroupedSpanSizeLookup(adapter, ColumnsCount));
            _recyclerView.SetLayoutManager(layoutManager);

            _generateButton.SetCommand(ViewModel.GenerateGroupCommand);
            _addButton.SetCommand(ViewModel.AddAllToCartCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ProductBasketViewModel.Status, () => SupportActionBar!.Title);
            this.Bind(() => ViewModel.ProductListViewModel.IsBusy, () => _progress.Visibility, VisibilityConverter.Gone);
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
    ///     Sample of custom <see cref="BindableRecyclerViewAdapter{TKey,TItem,TItemHolder}"/>.
    /// </summary>
    internal class CustomAdapter : BindableRecyclerViewAdapter<
        ProductHeaderViewModel, // header data type
        ProductViewModel, // item data type
        ProductViewHolder> // item ViewHolder type
    {
        public CustomAdapter(ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> items, Type headerSectionViewHolder, Type footerSectionViewHolder)
            : base(items, headerSectionViewHolder: headerSectionViewHolder, footerSectionViewHolder: footerSectionViewHolder)
        {
        }

        // Optional: custom inflating for ViewHolders without BindableViewHolderLayout attribute
        protected override View GetCustomLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            var textView = new TextView(parent.Context);
            var lp = new RecyclerView.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            textView.LayoutParameters = lp;

            return textView;
        }
    }
}
