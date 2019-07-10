// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.EventArguments;
using Softeq.XToolkit.Common.WeakSubscription;
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
        private readonly WeakReferenceEx<UITableView> _tableViewRef;
        private IDisposable _subscription;
        private readonly Func<nint, nint> _getRowInSectionCountFunc;
        private readonly Func<NSIndexPath, nfloat> _getHeightForRowFunc;

        public ObservableGroupTableViewSource(
            UITableView tableView,
            ObservableKeyGroupsCollection<TKey, TItem> items,
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
            _subscription = new NotifyCollectionKeyGroupChangedEventSubscription(DataSource, NotifierCollectionChanged);
        }

        public ObservableKeyGroupsCollection<TKey, TItem> DataSource { get; }

        public nfloat? HeightForHeader { get; set; }

        public nfloat? HeightForFooter { get; set; }
        public nfloat? HeightForRow { get; set; }

        /// <summary>
        /// Called when item was selected
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemSelected;

        /// <summary>
        /// Called every time when user clicked by item (select/deselect)
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemTapped;

        public event EventHandler LastItemRequested;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemByIndex(indexPath);

            if (indexPath.Section == DataSource.Count - 1 && indexPath.Row == DataSource[indexPath.Section].Count - 1)
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }

            return _getCellViewFunc.Invoke(tableView, item, DataSource[indexPath.Section], indexPath);
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
            return DataSource.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _getRowInSectionCountFunc?.Invoke(section) ?? DataSource[(int) section].Count;
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

        private TItem GetItemByIndex(NSIndexPath indexPath)
        {
            return DataSource[indexPath.Section][indexPath.Row];
        }

        private TKey GetKeyBySection(nint section)
        {
            return DataSource[(int) section].Key;
        }

        private void NotifierCollectionChanged(object sender, NotifyKeyGroupsCollectionChangedEventArgs e)
        {
            Execute(() =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Remove)
                {
                    _tableViewRef.Target?.ReloadData();
                    return;
                }

                _tableViewRef.Target?.BeginUpdates();

                var modifiedSectionsIndexes = e.ModifiedSectionsIndexes.OrderBy(x => x);

                foreach (var sectionIndex in modifiedSectionsIndexes)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        PerformWithoutAnimation(() =>
                        {
                            _tableViewRef.Target?.InsertSections(NSIndexSet.FromIndex(sectionIndex),
                                UITableViewRowAnimation.None);
                        });
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        _tableViewRef.Target?.DeleteSections(NSIndexSet.FromIndex(sectionIndex), UITableViewRowAnimation.None);
                    }
                }

                var modifiedIndexPaths = new List<NSIndexPath>();
                foreach (var (section, modifiedIndexes) in e.ModifiedItemsIndexes)
                {
                    foreach (var insertedItemIndex in modifiedIndexes)
                    {
                        modifiedIndexPaths.Add(NSIndexPath.FromRowSection(insertedItemIndex, section));
                    }
                }

                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    PerformWithoutAnimation(() =>
                    {
                        _tableViewRef.Target?.InsertRows(modifiedIndexPaths.ToArray(), UITableViewRowAnimation.None);
                    });
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    _tableViewRef.Target?.DeleteRows(modifiedIndexPaths.ToArray(), UITableViewRowAnimation.None);
                }

                _tableViewRef.Target?.EndUpdates();
            });
        }

        private static void Execute(Action action)
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

        private static void PerformWithoutAnimation(Action action)
        {
            UIView.PerformWithoutAnimation(action);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (_getHeightForRowFunc != null)
            {
                return _getHeightForRowFunc(indexPath);
            }

            return HeightForRow ?? 0;
        }
    }
}
