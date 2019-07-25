// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView
{
    public interface IBindableCollectionViewSource
    {
        object GetItemAt(int index);
    }

    public class BindableCollectionViewSource<T> : ObservableCollectionViewSource<T, UICollectionViewCell>,
        IBindableCollectionViewSource
    {
        private Func<UICollectionView, NSIndexPath, UICollectionViewCell> GetCellDelegate { get; set; }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = GetCellDelegate.Invoke(collectionView, indexPath);

            var bindableCell = (IBindable) cell;
            bindableCell.SetDataContext(DataSource[indexPath.Row]);

            return cell;
        }

        public object GetItemAt(int index)
        {
            return DataSource[index];
        }
    }
}