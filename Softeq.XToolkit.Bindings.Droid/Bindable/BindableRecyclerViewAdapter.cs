// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Softeq.XToolkit.Bindings.Droid.Handlers;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Weak;

#nullable disable

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public abstract class BindableRecyclerViewAdapterBase<TItem, TItemHolder> : RecyclerView.Adapter
        where TItemHolder : BindableViewHolder<TItem>
    {
        protected readonly IList<FlatItem> _flatMapping = new List<FlatItem>();

        protected IDisposable _subscription;
        private protected ICommand<TItem> _itemClick;

        public BindableRecyclerViewAdapterBase(
            Type headerViewHolder,
            Type footerViewHolder)
        {
            HeaderViewHolder = headerViewHolder;
            FooterViewHolder = footerViewHolder;
        }

        protected Type HeaderViewHolder { get; set; }

        protected Type FooterViewHolder { get; set; }

        public ICommand<TItem> ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null && value != null)
                {
                    throw new ArgumentException(
                        "Changing ItemClick may cause inconsistencies where some items still call the old command.",
                        nameof(ItemClick));
                }

                _itemClick = value;
            }
        }

        public override int GetItemViewType(int position)
        {
            return (int) _flatMapping[position].Type;
        }

        /// <summary>
        ///     By default, force recycling a view if it has animations.
        /// </summary>
        public override bool OnFailedToRecycleView(Java.Lang.Object holder)
        {
            return true;
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.OnAttachedToWindow();
            }
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.OnDetachedFromWindow();
            }

            base.OnViewDetachedFromWindow(holder);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemType = (ItemType) viewType;

            switch (itemType)
            {
                case ItemType.Header:
                    return OnCreateHeaderViewHolder(parent);

                case ItemType.Item:
                    return OnCreateItemViewHolder(parent, itemType);

                case ItemType.Footer:
                    return OnCreateFooterViewHolder(parent);

                default:
                    throw new ArgumentException($"Unable to create a view holder for \"{viewType}\" view type.", nameof(viewType));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;
            var flatItem = _flatMapping[position];
            var dataContext = GetDataContext(flatItem);

            bindableViewHolder.ReloadDataContext(dataContext);

            if (flatItem.Type == ItemType.Item)
            {
                bindableViewHolder.ItemClicked -= OnItemViewClick;
                bindableViewHolder.ItemClicked += OnItemViewClick;
            }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.ItemClicked -= OnItemViewClick;
                bindableViewHolder.OnViewRecycled();
            }

            base.OnViewRecycled(holder);
        }

        protected virtual RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, HeaderViewHolder);
        }

        protected virtual TItemHolder OnCreateItemViewHolder(ViewGroup parent, ItemType itemType)
        {
            return (TItemHolder) CreateViewHolder(parent, typeof(TItemHolder));
        }

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            var bindableViewHolder = (IBindableViewHolder) sender;

            ExecuteCommandOnItem(ItemClick, bindableViewHolder.DataContext);
        }

        protected virtual RecyclerView.ViewHolder OnCreateFooterViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, FooterViewHolder);
        }

        protected virtual RecyclerView.ViewHolder CreateViewHolder(ViewGroup parent, Type viewHolderType)
        {
            var view = GetLayoutForViewHolder(parent, viewHolderType);
            return (RecyclerView.ViewHolder) Activator.CreateInstance(viewHolderType, view);
        }

        protected virtual View GetLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            if (viewHolderType == null)
            {
                throw new ArgumentNullException(nameof(viewHolderType), "Check ViewHolder declarations.");
            }

            if (Attribute.GetCustomAttribute(viewHolderType, typeof(BindableViewHolderLayoutAttribute))
                is BindableViewHolderLayoutAttribute attr)
            {
                return LayoutInflater.From(parent.Context).Inflate(attr.LayoutId, parent, false);
            }

            return GetCustomLayoutForViewHolder(parent, viewHolderType);
        }

        protected virtual View GetCustomLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            throw new NotImplementedException(
                "Tried to use custom inflating of ViewHolder layout, please implement this method. " +
                $"Or use {nameof(BindableViewHolderLayoutAttribute)} for auto-inflating ViewHolder layout.");
        }

        protected virtual void ExecuteCommandOnItem(ICommand<TItem> command, object itemDataContext)
        {
            if (command != null && itemDataContext != null && command.CanExecute(itemDataContext))
            {
                command.Execute(itemDataContext);
            }
        }

        protected abstract object GetDataContext(FlatItem flatItem);
    }

    public class BindableRecyclerViewAdapter<TItem, TItemHolder> : BindableRecyclerViewAdapterBase<TItem, TItemHolder>
        where TItemHolder : BindableViewHolder<TItem>
    {
        private readonly IEnumerable<TItem> _dataSource;

        public BindableRecyclerViewAdapter(
            IEnumerable<TItem> items,
            Type headerViewHolder = null,
            Type footerViewHolder = null)
            : base(headerViewHolder, footerViewHolder)
        {
            _dataSource = items;

            if (_dataSource is INotifyCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionChangedEventSubscription(dataSource, NotifyCollectionChanged);
            }

            ReloadMapping();
        }

        public bool ShouldNotifyByAction { get; set; }

        public override int ItemCount => _flatMapping.Count;

        private void ReloadMapping()
        {
            _flatMapping.Clear();

            if (HeaderViewHolder != null)
            {
                _flatMapping.Add(FlatItem.CreateForHeader());
            }

            for (int index = 0; index < _dataSource.Count(); index++)
            {
                _flatMapping.Add(FlatItem.CreateForItem(0, index));
            }

            if (FooterViewHolder != null)
            {
                _flatMapping.Add(FlatItem.CreateForFooter());
            }
        }

        protected override object GetDataContext(FlatItem flatItem)
        {
            switch (flatItem.Type)
            {
                case ItemType.Item:
                    return _dataSource.ElementAt(flatItem.ItemIndex);

                default:
                    return null;
            }
        }

        private void NotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ReloadMapping();

            NotifyCollectionChangedOnMainThread(e);
        }

        private void NotifyCollectionChangedOnMainThread(NotifyCollectionChangedEventArgs e)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyAdapterSource(e);
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => NotifyAdapterSource(e));
            }
        }

        protected virtual void NotifyAdapterSource(NotifyCollectionChangedEventArgs e)
        {
            if (ShouldNotifyByAction)
            {
                NotifyCollectionChangedByAction(e);
            }
            else
            {
                NotifyDataSetChanged();
            }
        }

        protected virtual void NotifyCollectionChangedByAction(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Move:
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        NotifyItemMoved(e.OldStartingIndex + i, e.NewStartingIndex + i);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
            }
        }
    }

    public class BindableRecyclerViewAdapter<TKey, TItem, TItemHolder> : BindableRecyclerViewAdapterBase<TItem, TItemHolder>
        where TItemHolder : BindableViewHolder<TItem>
    {
        private readonly IEnumerable<IGrouping<TKey, TItem>> _dataSource;

        public BindableRecyclerViewAdapter(
            IEnumerable<IGrouping<TKey, TItem>> items,
            Type headerViewHolder = null,
            Type footerViewHolder = null)
            : base(headerViewHolder, footerViewHolder)
        {
            _dataSource = items;

            if (_dataSource is INotifyGroupCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(dataSource, NotifyCollectionChanged);
            }
            else if (_dataSource is INotifyKeyGroupCollectionChanged<TKey, TItem> dataSourceNew)
            {
                _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(dataSourceNew, NotifyCollectionChangedNew);
            }

            ReloadMapping();
        }

        public Type HeaderSectionViewHolder { get; set; }

        public Type FooterSectionViewHolder { get; set; }

        public override int ItemCount => _flatMapping.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemType = (ItemType) viewType;

            switch (itemType)
            {
                case ItemType.SectionHeader:
                    return OnCreateSectionHeaderViewHolder(parent, ItemType.SectionHeader);
                case ItemType.SectionFooter:
                    return OnCreateSectionFooterViewHolder(parent, ItemType.SectionFooter);

                default:
                    return base.OnCreateViewHolder(parent, viewType);
            }
        }

        protected override object GetDataContext(FlatItem flatItem)
        {
            switch (flatItem.Type)
            {
                case ItemType.SectionHeader:
                case ItemType.SectionFooter:
                    return _dataSource.ElementAt(flatItem.SectionIndex).Key;

                case ItemType.Item:
                    return _dataSource.ElementAt(flatItem.SectionIndex).ElementAt(flatItem.ItemIndex);

                default:
                    return null;
            }
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionHeaderViewHolder(ViewGroup parent, ItemType itemType)
        {
            return CreateViewHolder(parent, HeaderSectionViewHolder);
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionFooterViewHolder(ViewGroup parent, ItemType itemType)
        {
            return CreateViewHolder(parent, FooterSectionViewHolder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscription?.Dispose();
        }

        private void ReloadMapping()
        {
            _flatMapping.Clear();

            if (HeaderViewHolder != null)
            {
                _flatMapping.Add(FlatItem.CreateForHeader());
            }

            for (int sectionIndex = 0; sectionIndex < _dataSource.Count(); sectionIndex++)
            {
                if (HeaderSectionViewHolder != null)
                {
                    _flatMapping.Add(FlatItem.CreateForSectionHeader(sectionIndex));
                }

                var sectionItemsCount = _dataSource.ElementAt(sectionIndex).Count();

                for (int itemIndex = 0; itemIndex < sectionItemsCount; itemIndex++)
                {
                    _flatMapping.Add(FlatItem.CreateForItem(sectionIndex, itemIndex));
                }

                if (FooterSectionViewHolder != null)
                {
                    _flatMapping.Add(FlatItem.CreateForSectionFooter(sectionIndex));
                }
            }

            if (FooterViewHolder != null)
            {
                _flatMapping.Add(FlatItem.CreateForFooter());
            }
        }

        #region BindableGroupRecyclerViewAdapter

        private void NotifyCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            ReloadMapping();

            NotifyCollectionChangedOnMainThread(e);
        }

        private void NotifyCollectionChangedOnMainThread(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyCollectionChangedByAction(e);
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => NotifyCollectionChangedByAction(e));
            }
        }

        protected virtual void NotifyCollectionChangedByAction(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            // TODO YP: improve handling without reload
            NotifyDataSetChanged();
        }

        #endregion

        #region BindableGroupRecyclerViewAdapterNew

        private void NotifyCollectionChangedNew(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            NotifyCollectionChangedOnMainThread(e);
        }

        private void NotifyCollectionChangedOnMainThread(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyCollectionChangedByAction(e);
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => NotifyCollectionChangedByAction(e));
            }
        }

        protected virtual void NotifyCollectionChangedByAction(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            DroidRecyclerDataSourceHandler.Handle(
                this,
                _dataSource,
                _flatMapping,
                HeaderSectionViewHolder != null,
                FooterSectionViewHolder != null,
                e);

            ReloadMapping();
        }

        #endregion
    }
}
