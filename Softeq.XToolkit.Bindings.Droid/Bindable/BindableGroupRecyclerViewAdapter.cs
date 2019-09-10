// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public class BindableGroupRecyclerViewAdapter<TKey, TItem, TItemHolder> : RecyclerView.Adapter
        where TItemHolder : BindableViewHolder<TItem>
    {
        private readonly IList<FlatItem> _flatMapping = new List<FlatItem>();
        private readonly IEnumerable<IGrouping<TKey, TItem>> _dataSource;
        private readonly IDisposable _subscription;

        private ICommand<TItem> _itemClick;

        public BindableGroupRecyclerViewAdapter(IEnumerable<IGrouping<TKey, TItem>> items)
        {
            _dataSource = items;

            if(_dataSource is INotifyGroupCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(dataSource, NotifyCollectionChanged);
            }
            else if(_dataSource is INotifyKeyGroupCollectionChanged<TKey, TItem> dataSourceNew)
            {
                _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(dataSourceNew, NotifyCollectionChangedNew);
            }

            ReloadMapping();
        }

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

        public Type HeaderViewHolder { get; set; }

        public Type HeaderSectionViewHolder { get; set; }

        public Type FooterSectionViewHolder { get; set; }

        public Type FooterViewHolder { get; set; }

        public override int ItemCount => _flatMapping.Count;

        public override int GetItemViewType(int position)
        {
            return (int) _flatMapping[position].Type;
        }

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
                    return OnCreateItemViewHolder(parent, itemType);

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
            var bindableViewHolder = (IBindableViewHolder) holder;
            var flatItem = _flatMapping[position];
            var dataContext = GetDataContext(flatItem.Type, flatItem.SectionIndex, flatItem.ItemIndex);

            bindableViewHolder.ReloadDataContext(dataContext);

            if (flatItem.Type == ItemType.Item)
            {
                bindableViewHolder.ItemClicked -= OnItemViewClick;
                bindableViewHolder.ItemClicked += OnItemViewClick;
            }
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

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            if (holder is IBindableViewHolder bindableViewHolder)
            {
                bindableViewHolder.ItemClicked -= OnItemViewClick;
                bindableViewHolder.OnViewRecycled();
            }

            base.OnViewRecycled(holder);
        }

        /// <summary>
        ///     By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Java.Lang.Object holder)
        {
            return true;
        }

        protected virtual object GetDataContext(ItemType type, int sectionIndex, int itemIndex)
        {
            switch (type)
            {
                case ItemType.SectionHeader:
                case ItemType.SectionFooter:
                    return _dataSource.ElementAt(sectionIndex).Key;

                case ItemType.Item:
                    return _dataSource.ElementAt(sectionIndex).ElementAt(itemIndex);

                default:
                    return null;
            }
        }

        protected virtual RecyclerView.ViewHolder OnCreateHeaderViewHolder(ViewGroup parent)
        {
            return CreateViewHolder(parent, HeaderViewHolder);
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionHeaderViewHolder(ViewGroup parent, ItemType itemType)
        {
            return CreateViewHolder(parent, HeaderSectionViewHolder);
        }

        protected virtual TItemHolder OnCreateItemViewHolder(ViewGroup parent, ItemType itemType)
        {
            return (TItemHolder) CreateViewHolder(parent, typeof(TItemHolder));
        }

        protected virtual RecyclerView.ViewHolder OnCreateSectionFooterViewHolder(ViewGroup parent, ItemType itemType)
        {
            return CreateViewHolder(parent, FooterSectionViewHolder);
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

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            var bindableViewHolder = (IBindableViewHolder) sender;

            ExecuteCommandOnItem(ItemClick, bindableViewHolder.DataContext);
        }

        protected virtual void ExecuteCommandOnItem(ICommand<TItem> command, object itemDataContext)
        {
            if (command != null && itemDataContext != null && command.CanExecute(itemDataContext))
            {
                command.Execute(itemDataContext);
            }
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
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                HandleGroupsReset();
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleGroupsAdd(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleGroupsRemove(e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    HandleGroupsReplace(e);
                    break;
            }

            if (e.GroupEvents != null)
            {
                foreach (var groupEvent in e.GroupEvents)
                {
                    switch (groupEvent.Arg.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            HandleItemsAdd(groupEvent.GroupIndex, groupEvent.Arg);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            HandleItemsRemove(groupEvent.GroupIndex, groupEvent.Arg);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            HandleItemsReset(groupEvent.GroupIndex);
                            break;
                    }
                }
            }

            ReloadMapping();
        }

        private void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            foreach (var range in e.NewItemRanges)
            {
                Enumerable.Range(range.Index, range.NewItems.Count())
                    .Apply(InsertSection);
            }
        }

        private void HandleGroupsRemove(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            foreach (var range in e.OldItemRanges)
            {
                Enumerable.Range(range.Index, range.OldItems.Count())
                    .Apply(RemoveSection);
            }
        }

        private void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            HandleGroupsAdd(e);
            HandleGroupsRemove(e);
        }

        private void HandleGroupsReset()
        {
            NotifyDataSetChanged();
        }

        private void HandleItemsAdd(int groupIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                InsertItems(groupIndex, range.Index, range.NewItems.Count());
            }
        }

        private void HandleItemsRemove(int groupIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                RemoveItems(groupIndex, range.Index, range.OldItems.Count());
            }
        }

        private void HandleItemsReset(int groupIndex)
        {
            RemoveItems(groupIndex, 0, _flatMapping.Count(x => x.SectionIndex == groupIndex && x.Type == ItemType.Item));
        }

        private void InsertSection(int sectionIndex)
        {
            int positionStart = default;
            int count = default;

            var flat = _flatMapping.LastOrDefault(x => x.SectionIndex == sectionIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.Type == ItemType.Header);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat) + 1;
            }

            if (HeaderSectionViewHolder != null)
            {
                count += 1;
            }

            count += _dataSource.ElementAt(sectionIndex).Count();

            if (FooterSectionViewHolder != null)
            {
                count += 1;
            }

            NotifyItemRangeInserted(positionStart, count);
        }

        private void RemoveSection(int sectionIndex)
        {
            int positionStart = default;
            int count = default;

            var flat = _flatMapping.FirstOrDefault(x => x.SectionIndex == sectionIndex);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat);
            }

            count = _flatMapping.Count(x => x.SectionIndex == sectionIndex);

            NotifyItemRangeRemoved(positionStart, count);
        }

        private void InsertItems(int sectionIndex, int startIndex, int count)
        {
            int positionStart = default;

            var flat = _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.ItemIndex == startIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.Type == ItemType.SectionHeader)
                ?? _flatMapping?.LastOrDefault(x => x.SectionIndex == sectionIndex - 1)
                ?? _flatMapping?.FirstOrDefault(x => x.Type == ItemType.Header);

            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat) + 1;
            }

            NotifyItemRangeInserted(positionStart, count);
        }

        private void RemoveItems(int sectionIndex, int startIndex, int count)
        {
            int positionStart = default;

            var flat = _flatMapping?.FirstOrDefault(x => x.SectionIndex == sectionIndex && x.ItemIndex == startIndex);
            if (flat == null)
            {
                positionStart = 0;
            }
            else
            {
                positionStart = _flatMapping.IndexOf(flat);
            }

            NotifyItemRangeRemoved(positionStart, count);
        }

        #endregion
    }
}
