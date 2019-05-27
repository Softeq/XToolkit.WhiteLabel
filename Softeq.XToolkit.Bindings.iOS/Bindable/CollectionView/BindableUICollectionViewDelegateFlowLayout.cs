// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView
{
    public class BindableUICollectionViewDelegateFlowLayout : UICollectionViewDelegateFlowLayout
    {
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ((UICollectionViewSource) collectionView.DataSource)?.ItemSelected(collectionView, indexPath);
        }

        public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ((UICollectionViewSource) collectionView.DataSource)?.ItemDeselected(collectionView, indexPath);
        }
    }
}