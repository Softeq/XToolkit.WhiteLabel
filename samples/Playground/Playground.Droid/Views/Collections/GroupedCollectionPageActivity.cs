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

    public class BindableGroupRecyclerViewAdapter<TItem, TKey, TItemHolder, THeaderHolder> : RecyclerView.Adapter
    {
        protected const int DefaultViewType = 0;

        private readonly ObservableKeyGroupsCollection<TKey, TItem> _dataSource;
        private readonly int _itemLayoutId;
        private readonly int _headerLayoutId;

        private IDisposable _subscription;

        public BindableGroupRecyclerViewAdapter(
            ObservableKeyGroupsCollection<TKey, TItem> items,
            int itemLayoutId,
            int headerLayoutId = -1)
        {
            _dataSource = items;
            _itemLayoutId = itemLayoutId;
            _headerLayoutId = headerLayoutId;
            _subscription = new NotifyCollectionChangedEventSubscription(_dataSource, NotifierCollectionChanged);
        }

        public override int ItemCount => _dataSource.Keys.Count + _dataSource.Values.Count();

        //public abstract int GetSectionsCount();

        //public abstract int GetSectionItemsCount(int section);


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemType = (ItemType) viewType;

            switch (itemType)
            {
                case ItemType.Header:
                    return OnCreateHeaderViewHolder(parent);

                case ItemType.SectionHeader:
                    return OnCreateSectionHeaderViewHolder(parent, itemType);

                case ItemType.Item:
                    var viewHolder = OnCreateItemViewHolder(parent, itemType);
                    //viewHolder.ItemView.NotNull().ClickWeakSubscribe(ItemView_Click);
                    return viewHolder;

                case ItemType.SectionFooter:
                    return OnCreateSectionFooterViewHolder(parent, itemType);

                case ItemType.Footer:
                    return OnCreateFooterViewHolder(parent);

                default:
                    throw new ArgumentException($"Unable to create a view holder for \"{viewType}\" view type.", nameof(viewType));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bindableViewHolder = (IBindableView) holder;

            var data = new object();

            bindableViewHolder.ReloadDataContext(data);

            // TODO: 
        }

        protected virtual RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            // TODO:
            return null;
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionHeaderViewHolder(ViewGroup parent, ItemType itemType)
        {
            // TODO:
            return null;
        }

        protected virtual RecyclerView.ViewHolder OnCreateItemViewHolder(ViewGroup parent, ItemType itemType)
        {
            // TODO:
            return null;
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionFooterViewHolder(ViewGroup parent, ItemType itemType)
        {
            // TODO:
            return null;
        }

        protected virtual RecyclerView.ViewHolder OnCreateFooterViewHolder(ViewGroup parent)
        {
            // TODO:
            return null;
        }

        private void NotifierCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO:
        }
    }

    public enum ItemType
    {
        Header = 1,
        SectionHeader = 2,
        Item = 3,
        SectionFooter = 4,
        Footer = 5
    }
}
