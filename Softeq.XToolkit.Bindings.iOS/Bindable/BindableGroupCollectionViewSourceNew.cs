﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Extensions;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.WeakSubscription;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableGroupCollectionViewSourceNew<TKey, TItem, THeaderView, TItemCell> : UICollectionViewSource
        where TItem : class
        where THeaderView : BindableUICollectionReusableView<TKey>
        where TItemCell : BindableCollectionViewCell<TItem>
    {
        private readonly IDisposable _subscription;

        private WeakReferenceEx<UICollectionView> _collectionViewRef;
        private ICommand<TItem> _itemClick;

        public BindableGroupCollectionViewSourceNew(ObservableKeyGroupsCollectionNew<TKey, TItem> items)
        {
            DataSource = items;
            _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(DataSource, NotifierCollectionChanged);
        }

        public ObservableKeyGroupsCollectionNew<TKey, TItem> DataSource { get; }

        public ICommand<TItem> ItemClick
        {
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

        /// <inheritdoc />
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            if (_collectionViewRef == null)
            {
                _collectionViewRef = WeakReferenceEx.Create(collectionView);
            }

            return DataSource.Count();
        }

        /// <inheritdoc />
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return DataSource.ElementAt((int) section).Count();
        }

        /// <inheritdoc />
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TItemCell) collectionView.DequeueReusableCell(typeof(TItemCell).Name, indexPath);
            var bindableCell = (IBindableView) cell;

            bindableCell.ReloadDataContext(GetItemByIndexPath(indexPath));

            return cell;
        }

        /// <inheritdoc />
        public override UICollectionReusableView GetViewForSupplementaryElement(
            UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (elementKind == UICollectionElementKindSectionKey.Header)
            {
                return GetHeaderView(collectionView, indexPath);
            }

            if (elementKind == UICollectionElementKindSectionKey.Footer)
            {
                return GetFooterView(collectionView, indexPath);
            }

            return GetCustomSupplementaryView(collectionView, elementKind, indexPath);
        }

        /// <inheritdoc />
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _itemClick?.Execute(GetItemByIndexPath(indexPath));
        }

        /// <inheritdoc />
        public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        {
        }

        /// <summary>
        ///     Must return a valid <see cref="THeaderView"/> view for section header.
        ///
        ///     This method must always return a valid view object. If you do not want a supplementary view in
        ///     a particular case, your layout object should not create the attributes for that view.
        /// </summary>
        /// <param name="collectionView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        protected virtual UICollectionReusableView GetHeaderView(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var header = collectionView.DequeueReusableSupplementaryView(
                UICollectionElementKindSectionKey.Header,
                typeof(THeaderView).Name,
                indexPath);

            var bindableHeader = (IBindableView) header;

            bindableHeader.ReloadDataContext(DataSource.ElementAt(indexPath.Section).Key);

            return header;
        }

        /// <summary>
        ///     Must return a valid view for section footer.
        ///
        ///     This method must always return a valid view object. If you do not want a supplementary view in
        ///     a particular case, your layout object should not create the attributes for that view.
        /// </summary>
        /// <param name="collectionView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        protected virtual UICollectionReusableView GetFooterView(UICollectionView collectionView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Must return a valid supplementary view for section.
        ///
        ///     This method must always return a valid view object. If you do not want a supplementary view in
        ///     a particular case, your layout object should not create the attributes for that view.
        /// </summary>
        /// <param name="collectionView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        protected virtual UICollectionReusableView GetCustomSupplementaryView(
            UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        protected virtual TItem GetItemByIndexPath(NSIndexPath indexPath)
        {
            return DataSource.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
        }

        protected virtual void NotifierCollectionChanged(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    HandleGroupsReset();
                    return;
                }

                _collectionViewRef.Target?.PerformBatchUpdates(() =>
                {
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
                }, null);
            });
        }

        private void HandleGroupsAdd(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            foreach (var sectionsRange in e.NewItemRanges)
            {
                int sectionIndex = sectionsRange.Index;

                foreach (var section in sectionsRange.NewItems)
                {
                    _collectionViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex));

                    sectionIndex++;
                }
            }
        }

        private void HandleGroupsRemove(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            foreach (var sectionsRange in e.OldItemRanges)
            {
                int sectionIndex = sectionsRange.Index;

                foreach (var section in sectionsRange.OldItems)
                {
                    _collectionViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex));

                    sectionIndex++;
                }
            }
        }

        private void HandleGroupsReplace(NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            HandleGroupsAdd(e);
            HandleGroupsRemove(e);
        }

        private void HandleGroupsReset()
        {
            _collectionViewRef.Target?.ReloadData();
        }

        private void HandleItemsAdd(int groupIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.NewItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.NewItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, groupIndex))
                    .ToArray();
                _collectionViewRef.Target?.InsertItems(indexPaths);
            }
        }

        private void HandleItemsRemove(int groupIndex, NotifyGroupCollectionChangedArgs<TItem> args)
        {
            foreach (var range in args.OldItemRanges)
            {
                var indexPaths = Enumerable.Range(range.Index, range.OldItems.Count)
                    .Select(x => NSIndexPath.FromRowSection(x, groupIndex))
                    .ToArray();
                _collectionViewRef.Target?.DeleteItems(indexPaths);
            }
        }

        private void HandleItemsReset(int groupIndex)
        {
            _collectionViewRef.Target?.ReloadSections(NSIndexSet.FromIndex(groupIndex));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscription.Dispose();
        }
    }
}
