// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
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
    public class BindableGroupCollectionViewSource<TKey, TItem, THeaderView, TItemCell> : UICollectionViewSource
        where TItem : class
        where THeaderView : BindableUICollectionReusableView<TKey>
        where TItemCell : BindableCollectionViewCell<TItem>
    {
        private readonly IDisposable _subscription;

        private WeakReferenceEx<UICollectionView> _collectionViewRef;
        private ICommand<TItem> _itemClick;

        public BindableGroupCollectionViewSource(ObservableKeyGroupsCollection<TKey, TItem> items)
        {
            DataSource = items;
            _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(DataSource, NotifierCollectionChanged);
        }

        public ObservableKeyGroupsCollection<TKey, TItem> DataSource { get; }

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
            var cell = (TItemCell) collectionView.DequeueReusableCell(typeof(TItemCell).Name, indexPath);
            var bindableCell = (IBindableView) cell;

            bindableCell.ReloadDataContext(GetItemByIntexPath(indexPath));

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
            _itemClick?.Execute(GetItemByIntexPath(indexPath));
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

        protected virtual TItem GetItemByIntexPath(NSIndexPath indexPath)
        {
            return DataSource[indexPath.Section][indexPath.Row];
        }

        protected virtual void NotifierCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                _collectionViewRef.Target?.ReloadData();
            });
        }
    }
}
