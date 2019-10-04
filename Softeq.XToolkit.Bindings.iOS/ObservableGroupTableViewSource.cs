// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Extensions;
using Softeq.XToolkit.Bindings.iOS.Handlers;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Common.Weak;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    public class ObservableGroupTableViewSource<TKey, TItem> : UITableViewSource
    {
        private readonly Func<UITableView, TItem, IList<TItem>, NSIndexPath, UITableViewCell> _getCellViewFunc;
        private readonly Func<TKey, nfloat> _getFooterHeightFunc;
        private readonly Func<UITableView, TKey, UIView> _getFooterViewFunc;
        private readonly Func<TKey, nfloat> _getHeaderHeightFunc;
        private readonly Func<UITableView, TKey, UIView> _getHeaderViewFunc;
        private readonly Func<NSIndexPath, nfloat> _getHeightForRowFunc;
        private readonly Func<nint, nint> _getRowInSectionCountFunc;
        private readonly WeakReferenceEx<UITableView> _tableViewRef;
        private readonly IDisposable _subscription;

        public ObservableGroupTableViewSource(
            UITableView tableView,
            IEnumerable<IGrouping<TKey, TItem>> items,
            Func<UITableView, TItem, IList<TItem>, NSIndexPath, UITableViewCell> getCellViewFunc,
            Func<nint, nint> getRowInSectionCountFunc = null,
            Func<UITableView, TKey, UIView> getHeaderViewFunc = null,
            Func<UITableView, TKey, UIView> getFooterViewFunc = null,
            Func<TKey, nfloat> getHeaderHeightFunc = null,
            Func<TKey, nfloat> getFooterHeightFunc = null,
            Func<NSIndexPath, nfloat> getHeightForRowFunc = null)
        {
            _getCellViewFunc = getCellViewFunc;
            _getHeightForRowFunc = getHeightForRowFunc;
            _getRowInSectionCountFunc = getRowInSectionCountFunc;
            _getHeaderViewFunc = getHeaderViewFunc;
            _getFooterViewFunc = getFooterViewFunc;
            _getFooterHeightFunc = getFooterHeightFunc;
            _getHeaderHeightFunc = getHeaderHeightFunc;
            _tableViewRef = WeakReferenceEx.Create(tableView);

            DataSource = items;

            if (DataSource is INotifyGroupCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(dataSource, NotifyCollectionChanged);
            }
            else if (DataSource is INotifyKeyGroupCollectionChanged<TKey, TItem> dataSourceNew)
            {
                _subscription = new NotifyCollectionKeyGroupNewChangedEventSubscription<TKey, TItem>(dataSourceNew, NotifyCollectionChangedNew);
            }
        }

        public IEnumerable<IGrouping<TKey, TItem>> DataSource { get; }

        public nfloat? HeightForHeader { get; set; }

        public nfloat? HeightForFooter { get; set; }
        public nfloat? HeightForRow { get; set; }

        /// <summary>
        ///     Called when item was selected
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemSelected;

        /// <summary>
        ///     Called every time when user clicked by item (select/deselect)
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemTapped;

        public event EventHandler LastItemRequested;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemByIndex(indexPath);

            if (indexPath.Section == DataSource.Count() - 1 && indexPath.Row == DataSource.ElementAt(indexPath.Section).Count() - 1)
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }

            return GetItemCell(tableView, item, DataSource.ElementAt(indexPath.Section).ToList(), indexPath);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            return _getHeaderViewFunc?.Invoke(tableView, GetKeyBySection(section)) ?? new UIView();
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            return _getFooterViewFunc?.Invoke(tableView, GetKeyBySection(section)) ?? new UIView();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return DataSource.Count();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _getRowInSectionCountFunc?.Invoke(section) ?? DataSource.ElementAt((int) section).Count();
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (_getHeaderHeightFunc != null)
            {
                return _getHeaderHeightFunc.Invoke(GetKeyBySection(section));
            }

            return HeightForHeader ?? 0;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (_getFooterHeightFunc != null)
            {
                return _getFooterHeightFunc.Invoke(GetKeyBySection(section));
            }

            return HeightForFooter ?? 0;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (_getHeightForRowFunc != null)
            {
                return _getHeightForRowFunc(indexPath);
            }

            return HeightForRow ?? 0;
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemByIndex(indexPath);
            var args = new GenericEventArgs<TItem>(item);

            ItemTapped?.Invoke(this, args);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemByIndex(indexPath);
            var args = new GenericEventArgs<TItem>(item);

            ItemSelected?.Invoke(this, args);
            ItemTapped?.Invoke(this, args);
        }

        protected virtual UITableViewCell GetItemCell(UITableView tableView, TItem item, IList<TItem> items, NSIndexPath indexPath)
        {
            return _getCellViewFunc?.Invoke(tableView, item, items, indexPath);
        }

        protected TItem GetItemByIndex(NSIndexPath indexPath)
        {
            return DataSource.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
        }

        protected TKey GetKeyBySection(nint section)
        {
            return DataSource.ElementAt((int) section).Key;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscription?.Dispose();
        }

        private void HandleAdd(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            foreach (var sectionIndex in e.ModifiedSectionsIndexes)
            {
                _tableViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.None);
            }

            var rowsToInsert = CreateRowsChanges(e.ModifiedItemsIndexes);

            _tableViewRef.Target?.InsertRows(rowsToInsert, UITableViewRowAnimation.None);
        }

        private void HandleRemove(NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            foreach (var sectionIndex in e.ModifiedSectionsIndexes)
            {
                _tableViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.None);
            }

            var rowsToRemove = CreateRowsChanges(e.ModifiedItemsIndexes);

            _tableViewRef.Target?.DeleteRows(rowsToRemove, UITableViewRowAnimation.None);
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

        #region ObservableKeyGroupsCollection

        protected void NotifyCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Remove)
                {
                    _tableViewRef.Target?.ReloadData();
                    return;
                }

                _tableViewRef.Target?.BeginUpdates();

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        HandleAdd(e);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        HandleRemove(e);
                        break;
                }

                _tableViewRef.Target?.EndUpdates();
            });
        }
        #endregion

        #region ObservableKeyGroupsCollectionNew

        protected void NotifyCollectionChangedNew(object sender, NotifyKeyGroupCollectionChangedEventArgs<TKey, TItem> e)
        {
            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                IosDataSourceHandler.Handle(_tableViewRef.Target, e);
            });
        }

        #endregion
    }
}
