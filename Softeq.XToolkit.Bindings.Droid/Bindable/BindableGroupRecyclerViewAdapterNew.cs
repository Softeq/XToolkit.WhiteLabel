// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public class BindableGroupRecyclerViewAdapterNew<TKey, TItem, TItemHolder> : RecyclerView.Adapter
        where TItemHolder : BindableViewHolder<TItem>
    {
        private readonly IList<FlatItem> _flatMapping = new List<FlatItem>();
        private readonly ObservableKeyGroupsCollectionNew<TKey, TItem> _dataSource;
        private readonly IDisposable _subscription;

        private ICommand<TItem> _itemClick;

        public BindableGroupRecyclerViewAdapterNew(
            ObservableKeyGroupsCollectionNew<TKey, TItem> items)
        {
            _dataSource = items;
            _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(_dataSource, NotifyCollectionChanged);

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

        protected virtual void NotifyCollectionChangedByAction(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            // TODO YP: improve handling without reload
            NotifyDataSetChanged();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscription.Dispose();
        }

        private void NotifyCollectionChanged(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            ReloadMapping();

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
    }
}
