// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Extensions;
using Softeq.XToolkit.Bindings.iOS.Handlers;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Weak;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public abstract class BindableTableViewSourceBase<TItem> : UITableViewSource
    {
        private protected readonly WeakReferenceEx<UITableView> _tableViewRef;

        private ICommand<TItem> _itemClick;

        private protected IDisposable _subscription;

        public event EventHandler LastItemRequested;

        protected BindableTableViewSourceBase(UITableView tableView)
        {
            _tableViewRef = WeakReferenceEx.Create(tableView);
        }

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

        public Func<NSIndexPath, nfloat> GetHeightForRowFunc { get; }

        public nfloat? HeightForRow { get; set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemByIndex(indexPath);

            if (IsLastItem(indexPath))
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }

            var itemCell = tableView.DequeueReusableCell(GetCellName(indexPath), indexPath);
            var bindableView = (IBindableView) itemCell;

            bindableView.ReloadDataContext(item);

            return itemCell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _itemClick?.Execute(GetItemByIndex(indexPath));
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (GetHeightForRowFunc != null)
            {
                return GetHeightForRowFunc(indexPath);
            }

            return HeightForRow ?? 0;
        }

        protected abstract TItem GetItemByIndex(NSIndexPath indexPath);

        protected abstract bool IsLastItem(NSIndexPath indexPath);

        protected abstract string GetCellName(NSIndexPath indexPath);
    }

    public abstract class BindableTableViewSourceBase<TItem, TItemCell> : BindableTableViewSourceBase<TItem>
        where TItemCell : BindableTableViewCell<TItem>
    {
        protected BindableTableViewSourceBase(UITableView tableView) : base(tableView)
        {
        }

        protected override string GetCellName(NSIndexPath indexPath) => typeof(TItemCell).Name;
    }

    public abstract class BindableTableViewSourceBase<TKey, TItem, TItemCell>
        : BindableTableViewSourceBase<TItem, TItemCell>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableTableViewSourceBase(
            UITableView tableView,
            IEnumerable<IGrouping<TKey, TItem>> items) : base(tableView)
        {
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

        public override nint NumberOfSections(UITableView tableView)
        {
            return DataSource.Count();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return DataSource.ElementAt((int) section).Count();
        }

        protected TKey GetKeyBySection(nint section)
        {
            return DataSource.ElementAt((int) section).Key;
        }

        protected override bool IsLastItem(NSIndexPath indexPath)
        {
            return indexPath.Section == DataSource.Count() - 1 &&
                   indexPath.Row == DataSource.ElementAt(indexPath.Section).Count() - 1;
        }

        protected override TItem GetItemByIndex(NSIndexPath indexPath)
        {
            return DataSource.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
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

    public class BindableTableViewSource<TItem, TItemCell>
        : BindableTableViewSourceBase<TItem, TItemCell>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableTableViewSource(
            UITableView tableView,
            IEnumerable<TItem> items) : base(tableView)
        {
            DataSource = items;

            if (DataSource is INotifyCollectionChanged dataSource)
            {
                _subscription = new NotifyCollectionChangedEventSubscription(dataSource, NotifyCollectionChanged);
            }
        }

        public IEnumerable<TItem> DataSource { get; }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return DataSource.Count();
        }

        protected override TItem GetItemByIndex(NSIndexPath indexPath)
        {
            return DataSource.ElementAt(indexPath.Row);
        }

        protected override bool IsLastItem(NSIndexPath indexPath)
        {
            return indexPath.Row == DataSource.Count() - 1;
        }

        private void NotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_tableViewRef.Target == null)
            {
                return;
            }

            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var count = e.NewItems.Count;
                            var paths = new NSIndexPath[count];

                            for (var i = 0; i < count; i++)
                            {
                                paths[i] = NSIndexPath.FromRowSection(e.NewStartingIndex + i, 0);
                            }

                            _tableViewRef.Target?.InsertRows(paths, UITableViewRowAnimation.Automatic);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        {
                            var count = e.OldItems.Count;
                            var paths = new NSIndexPath[count];

                            for (var i = 0; i < count; i++)
                            {
                                var index = NSIndexPath.FromRowSection(e.OldStartingIndex + i, 0);
                                paths[i] = index;
                            }

                            _tableViewRef.Target?.DeleteRows(paths, UITableViewRowAnimation.Automatic);
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        {
                            if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
                            {
                                _tableViewRef.Target?.ReloadData();
                                break;
                            }

                            if (e.NewStartingIndex != e.OldStartingIndex)
                            {
                                _tableViewRef.Target?.MoveRow(NSIndexPath.FromRowSection(e.OldStartingIndex, 0),
                                    NSIndexPath.FromRowSection(e.NewStartingIndex, 0));
                            }

                            break;
                        }

                    default:
                        _tableViewRef.Target?.ReloadData();
                        break;
                }
            });
        }
    }

    public class BindableTableViewSource<TKey, TItem, TGroupCell, TItemCell>
        : BindableTableViewSourceBase<TKey, TItem, TItemCell>
        where TGroupCell : BindableTableViewHeaderFooterView<TKey>
        where TItemCell : BindableTableViewCell<TItem>
    {
        private readonly UICollectionElementKindSection _kind;

        public BindableTableViewSource(
            UITableView tableView,
            UICollectionElementKindSection kind,
            IEnumerable<IGrouping<TKey, TItem>> items) : base(tableView, items)
        {
            _kind = kind;
        }

        public Func<TKey, nfloat> GetHeightForGroupCellFunc { get; set; }

        public nfloat? HeightForGroupCell { get; set; }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (_kind != UICollectionElementKindSection.Header)
            {
                return 0;
            }

            return GetHeightForGroupCell(section);
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (_kind != UICollectionElementKindSection.Footer)
            {
                return 0;
            }

            return GetHeightForGroupCell(section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (_kind != UICollectionElementKindSection.Header)
            {
                return null;
            }

            return GetHeaderFooterView(tableView, section);
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            if (_kind != UICollectionElementKindSection.Footer)
            {
                return null;
            }

            return GetHeaderFooterView(tableView, section);
        }

        private nfloat GetHeightForGroupCell(nint section)
        {
            if (GetHeightForGroupCellFunc != null)
            {
                return GetHeightForGroupCellFunc.Invoke(GetKeyBySection(section));
            }

            return HeightForGroupCell ?? 0;
        }

        private UIView GetHeaderFooterView(UITableView tableView, nint section)
        {
            var groupCell = tableView.DequeueReusableHeaderFooterView(GetHeaderViewName(section));
            var bindableView = (IBindableView) groupCell;

            bindableView.ReloadDataContext(GetKeyBySection(section));

            return groupCell;
        }

        protected virtual string GetHeaderViewName(nint section) => typeof(TGroupCell).Name;
    }

    public class BindableTableViewSource<TKey, TItem, THeaderView, TFooterView, TItemCell>
        : BindableTableViewSourceBase<TKey, TItem, TItemCell>
        where THeaderView : BindableTableViewHeaderFooterView<TKey>
        where TFooterView : BindableTableViewHeaderFooterView<TKey>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableTableViewSource(
            UITableView tableView,
            IEnumerable<IGrouping<TKey, TItem>> items) : base(tableView, items)
        {
        }

        public Func<TKey, nfloat> GetHeightForHeaderFunc { get; set; }

        public Func<TKey, nfloat> GetHeightForFooterFunc { get; set; }

        public nfloat? HeightForHeader { get; set; }

        public nfloat? HeightForFooter { get; set; }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (GetHeightForHeaderFunc != null)
            {
                return GetHeightForHeaderFunc.Invoke(GetKeyBySection(section));
            }

            return HeightForHeader ?? 0;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (GetHeightForFooterFunc != null)
            {
                return GetHeightForFooterFunc.Invoke(GetKeyBySection(section));
            }

            return HeightForFooter ?? 0;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerView = tableView.DequeueReusableHeaderFooterView(GetHeaderViewName(section));
            var bindableView = (IBindableView) headerView;

            bindableView.ReloadDataContext(GetKeyBySection(section));

            return headerView;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var footerView = tableView.DequeueReusableHeaderFooterView(GetFooterViewName(section));
            var bindableView = (IBindableView) footerView;

            bindableView.ReloadDataContext(GetKeyBySection(section));

            return footerView;
        }

        protected virtual string GetFooterViewName(nint section) => typeof(TFooterView).Name;

        protected virtual string GetHeaderViewName(nint section) => typeof(THeaderView).Name;
    }
}
