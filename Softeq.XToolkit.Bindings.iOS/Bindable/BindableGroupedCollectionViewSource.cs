// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.WeakSubscription;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableGroupedCollectionViewSource<TItem, TCell, TKey, THeaderView> : UICollectionViewSource
        where TCell : BindableCollectionViewCell<TItem>
        where THeaderView : BindableUICollectionReusableView<TKey>
    {
        private IDisposable _subscription;
        private WeakReferenceEx<UICollectionView> _collectionViewRef;

        public BindableGroupedCollectionViewSource(ObservableKeyGroupsCollection<TKey, TItem> items)
        {
            DataSource = items;
            _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(DataSource, NotifierCollectionChanged);
        }

        public ObservableKeyGroupsCollection<TKey, TItem> DataSource { get; }

        /// <inheritdoc />
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            if (_collectionViewRef == null)
            {
                _collectionViewRef = WeakReferenceEx.Create(collectionView);
            }

            return DataSource.Count;
        }

        /// <inheritdoc />
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return DataSource[(int) section].Count;
        }

        /// <inheritdoc />
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TCell) collectionView.DequeueReusableCell(typeof(TCell).Name, indexPath);
            var bindableCell = (IBindableView) cell;

            bindableCell.ReloadDataContext(DataSource[indexPath.Section][indexPath.Row]);

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

            bindableHeader.ReloadDataContext(DataSource[indexPath.Section].Key);

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

        protected void NotifierCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            Execute(() =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Remove)
                {
                    _collectionViewRef.Target?.ReloadData();
                    return;
                }

                _collectionViewRef.Target?.ReloadData();

                // TODO YP:
                //switch (e.Action)
                //{
                //    case NotifyCollectionChangedAction.Add:
                //        HandleAdd(e);
                //        break;
                //    case NotifyCollectionChangedAction.Remove:
                //        HandleRemove(e);
                //        break;
                //}
            });
        }

        protected static void Execute(Action action)
        {
            if (NSThread.IsMain)
            {
                action();
            }
            else
            {
                NSOperationQueue.MainQueue.AddOperation(action);
                NSOperationQueue.MainQueue.WaitUntilAllOperationsAreFinished();
            }
        }

        private void HandleAdd(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            foreach (var sectionIndex in e.ModifiedSectionsIndexes)
            {
                _collectionViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex));
            }

            var rowsToInsert = CreateRowsChanges(e.ModifiedItemsIndexes);

            _collectionViewRef.Target?.InsertItems(rowsToInsert);
        }

        private void HandleRemove(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            foreach (var sectionIndex in e.ModifiedSectionsIndexes)
            {
                _collectionViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex));
            }

            var rowsToRemove = CreateRowsChanges(e.ModifiedItemsIndexes);

            _collectionViewRef.Target?.DeleteItems(rowsToRemove);
        }

        private static NSIndexPath[] CreateRowsChanges(IEnumerable<(int Section, IList<int> ModifiedIndexes)> itemIndexes)
        {
            var modifiedIndexPaths = new List<NSIndexPath>();

            foreach (var (section, modifiedIndexes) in itemIndexes)
            {
                modifiedIndexPaths.AddRange(modifiedIndexes.Select(insertedItemIndex =>
                    NSIndexPath.FromRowSection(insertedItemIndex, section)));
            }

            return modifiedIndexPaths.ToArray();
        }
    }
}
