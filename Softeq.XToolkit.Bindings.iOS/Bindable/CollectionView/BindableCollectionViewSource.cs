using System;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable.Abstract;
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

            cell.SetDataContext(DataSource[indexPath.Row]);

            return cell;
        }

        public object GetItemAt(int index)
        {
            return DataSource[index];
        }
    }
}