// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Command;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView
{
    [Obsolete("Will be removed after removing BindableCollectionView.")]
    public interface IBindableCollectionViewSource
    {
        object GetItemAt(int index);
    }

    public class BindableCollectionViewSource<TViewModel, TCell> : ObservableCollectionViewSource<TViewModel, TCell>,
        IBindableCollectionViewSource
        where TCell : UICollectionViewCell, IBindable
    {
        private ICommand<TViewModel> _itemClick;

        public BindableCollectionViewSource(IList<TViewModel> items)
        {
            DataSource = items;
        }

        public ICommand<TViewModel> ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null && value != null)
                {
                    Debug.WriteLine("Changing ItemClick may cause inconsistencies where some items still call the old command.");
                }

                _itemClick = value;
            }
        }

        [Obsolete]
        public object GetItemAt(int index)
        {
            return DataSource[index];
        }

        /// <inheritdoc />
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TCell) collectionView.DequeueReusableCell(typeof(TCell).Name, indexPath);

            var bindableCell = (IBindable) cell;
            bindableCell.SetDataContext(DataSource[indexPath.Row]);

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
