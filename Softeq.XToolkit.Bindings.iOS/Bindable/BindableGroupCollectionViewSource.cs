// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Extensions;
using Softeq.XToolkit.Bindings.iOS.Handlers;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Weak;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableGroupCollectionViewSourceBase<TKey, TItem> : UICollectionViewSource
        where TItem : class
    {
        private readonly IDisposable _subscription;

        private WeakReferenceEx<UICollectionView> _collectionViewRef;
        private ICommand<TItem> _itemClick;

        public BindableGroupCollectionViewSourceBase(IEnumerable<IGrouping<TKey, TItem>> items)
        {
            DataSource = items;

            if (DataSource is INotifyGroupCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(dataSource, NotifierCollectionChanged);
            }
            else if (DataSource is INotifyKeyGroupCollectionChanged<TKey, TItem> dataSourceNew)
            {
                _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(dataSourceNew, NotifyCollectionChangedNew);
            }
        }

        public IEnumerable<IGrouping<TKey, TItem>> DataSource { get; }

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
            var cell = collectionView.DequeueReusableCell(GetCellName(indexPath), indexPath);
            var bindableCell = (IBindableView) cell;

            bindableCell.ReloadDataContext(GetItemByIndexPath(indexPath));

            return (UICollectionViewCell) cell;
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
        ///     Must return a valid view for section header.
        ///
        ///     This method must always return a valid view object. If you do not want a supplementary view in
        ///     a particular case, your layout object should not create the attributes for that view.
        /// </summary>
        /// <param name="collectionView">The collection view requesting this information.</param>
        /// <param name="indexPath">The index path that specifies the location of the new header view.</param>
        /// <returns>View for index.</returns>
        protected virtual UICollectionReusableView GetHeaderView(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var header = collectionView.DequeueReusableSupplementaryView(
                UICollectionElementKindSectionKey.Header,
                GetHeaderViewName(indexPath),
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
        /// <param name="collectionView">The collection view requesting this information.</param>
        /// <param name="indexPath">The index path that specifies the location of the new footer view.</param>
        /// <returns>View for index.</returns>
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
        /// <param name="collectionView">The collection view requesting this information.</param>
        /// <param name="elementKind">A string that identifies the type of supplementary item.</param>
        /// <param name="indexPath">The index path that specifies the location of the new supplementary view.</param>
        /// <returns>View for index.</returns>
        protected virtual UICollectionReusableView GetCustomSupplementaryView(
            UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        protected virtual TItem GetItemByIndexPath(NSIndexPath indexPath)
        {
            return DataSource.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscription?.Dispose();
        }

        #region BindableGroupRecyclerViewAdapter

        protected virtual void NotifierCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                // TODO YP: improve handling without reload
                _collectionViewRef.Target?.ReloadData();
            });
        }

        #endregion

        #region BindableGroupRecyclerViewAdapterNew

        protected virtual void NotifyCollectionChangedNew(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                IosDataSourceHandler.Handle(_collectionViewRef.Target, e);
            });
        }

        #endregion

        protected abstract string GetCellName(NSIndexPath indexPath);

        protected abstract string GetHeaderViewName(NSIndexPath indexPath);
    }

    public class BindableGroupCollectionViewSource<TKey, TItem, THeaderView, TItemCell>
        : BindableGroupCollectionViewSourceBase<TKey, TItem>
        where TItem : class
        where THeaderView : BindableUICollectionReusableView<TKey>
        where TItemCell : BindableCollectionViewCell<TItem>
    {
        public BindableGroupCollectionViewSource(IEnumerable<IGrouping<TKey, TItem>> items) : base(items)
        {
        }

        protected override string GetCellName(NSIndexPath indexPath) => typeof(TItemCell).Name;

        protected override string GetHeaderViewName(NSIndexPath indexPath) => typeof(THeaderView).Name;
    }
}
