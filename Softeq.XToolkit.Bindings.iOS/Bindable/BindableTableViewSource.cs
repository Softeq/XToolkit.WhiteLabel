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
        public BindableTableViewSource(IList<TItem> dataSource) : base(dataSource)
        {
            if (string.IsNullOrEmpty(ReuseId) || ReuseId == DefaultReuseId)
            {
                ReuseId = typeof(TItemCell).Name;
            }
        }

        /// <inheritdoc />
        protected override UITableViewCell GetItemCell(UITableView view, NSIndexPath indexPath)
        {
            var itemCell = view.DequeueReusableCell(NsReuseId, indexPath) ?? CreateCell(NsReuseId);
            var dataContext = GetItem(indexPath);

            BindCell(itemCell, dataContext, indexPath);

            return itemCell;
        }

        /// <inheritdoc />
        protected override void BindCell(UITableViewCell cell, object item, NSIndexPath indexPath)
        {
            var bindableView = (IBindableView) cell;
            bindableView.ReloadDataContext(item);
        }
    }
}
