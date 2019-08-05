// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Common.Collections;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableGroupTableViewSource<TKey, TItem, TGroupCell, TItemCell> : ObservableGroupTableViewSource<TKey, TItem>
        where TGroupCell : BindableHeaderCell<TKey>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableGroupTableViewSource(
            UITableView tableView,
            ObservableKeyGroupsCollection<TKey, TItem> items,
            Func<nint, nint> getRowInSectionCountFunc = null,
            Func<UITableView, TKey, UIView> getFooterViewFunc = null,
            Func<TKey, nfloat> getHeaderHeightFunc = null,
            Func<TKey, nfloat> getFooterHeightFunc = null,
            Func<NSIndexPath, nfloat> getHeightForRowFunc = null)
            : base(tableView, items, GetCellViewFunc, getRowInSectionCountFunc, GetHeaderViewFunc,
                  getFooterViewFunc, getHeaderHeightFunc, getFooterHeightFunc, getHeightForRowFunc)
        {
        }

        private static UITableViewCell GetCellViewFunc(UITableView tableView, TItem item, IList<TItem> dataSource, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(typeof(TItemCell).Name, indexPath);
            var itemCell = (TItemCell) cell;

            itemCell.DoDetachBindings();
            itemCell.DataContext = item;
            itemCell.DoAttachBindings();

            return itemCell;
        }

        private static UIView GetHeaderViewFunc(UITableView tableView, TKey headerKey)
        {
            var cell = tableView.DequeueReusableHeaderFooterView(typeof(TGroupCell).Name);
            var headerCell = (TGroupCell) cell;

            headerCell.DoDetachBindings();
            headerCell.DataContext = headerKey;
            headerCell.DoAttachBindings();

            return headerCell;
        }
    }
}
