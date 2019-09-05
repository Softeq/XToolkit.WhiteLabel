// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Collections;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableGroupTableViewSource<TKey, TItem, TGroupCell, TItemCell>
        : ObservableGroupTableViewSource<TKey, TItem>
        where TGroupCell : BindableTableViewHeaderFooterView<TKey>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableGroupTableViewSource(
            UITableView tableView,
            ObservableKeyGroupsCollection<TKey, TItem> items)
            : base(tableView, items, null)
        {
        }

        protected override UITableViewCell GetItemCell(
            UITableView tableView,
            TItem item,
            IList<TItem> items,
            NSIndexPath indexPath)
        {
            var itemCell = tableView.DequeueReusableCell(typeof(TItemCell).Name, indexPath);
            var bindableView = (IBindableView) itemCell;

            bindableView.ReloadDataContext(item);

            return itemCell;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerCell = tableView.DequeueReusableHeaderFooterView(typeof(TGroupCell).Name);
            var bindableView = (IBindableView) headerCell;

            bindableView.ReloadDataContext(GetKeyBySection(section));

            return headerCell;
        }
    }
}
