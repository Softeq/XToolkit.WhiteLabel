// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Commands;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS.Bindable
{
    public class BindableCollectionViewSource<TItem, TCell> : ObservableCollectionViewSource<TItem, TCell>
        where TCell : UICollectionViewCell, IBindableView
    {
        private ICommand<TItem> _itemClick;

        public BindableCollectionViewSource(IList<TItem> dataSource)
        {
            DataSource = dataSource;
            ReuseId = typeof(TCell).Name;
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

        /// <inheritdoc />
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TCell) collectionView.DequeueReusableCell(NsReuseId, indexPath);
            var bindableCell = (IBindableView) cell;

            bindableCell.ReloadDataContext(DataSource[indexPath.Row]);

            return cell;
        }

        /// <inheritdoc />
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);

            _itemClick?.Execute(DataSource[indexPath.Row]);
        }
    }
}
