// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableTableViewSource<TItem, TItemCell>
        : ObservableTableViewSource<TItem>
        where TItemCell : BindableTableViewCell<TItem>
    {
        public BindableTableViewSource(IList<TItem> items) : base(items)
        {
        }

        protected override UITableViewCell GetItemCell(UITableView view, NSIndexPath indexPath)
        {
            var cellReuseId = string.IsNullOrEmpty(NsReuseId) ? nameof(TItemCell) : NsReuseId;

            var itemCell = view.DequeueReusableCell(cellReuseId, indexPath) ?? CreateCell(NsReuseId);

            var item = DataSource[indexPath.Row];
            BindCell(itemCell, item, indexPath);

            return itemCell;
        }

        protected override void BindCell(UITableViewCell cell, object item, NSIndexPath indexPath)
        {
            var bindableView = (IBindableView) cell;
            bindableView.ReloadDataContext(item);
        }
    }
}
