// Developed by Softeq Development Corporation
// http://www.softeq.com

using CoreGraphics;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    internal class GroupedCollectionViewDelegateFlowLayout : BindableUICollectionViewDelegateFlowLayout
    {
        private readonly int _columnsCount;
        private readonly UIEdgeInsets _sectionInsets = new UIEdgeInsets(top: 20, left: 20, bottom: 20, right: 20);

        public GroupedCollectionViewDelegateFlowLayout(int columnsCount)
        {
            _columnsCount = columnsCount;
        }

        public override CGSize GetSizeForItem(
            UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var paddingSpace = _sectionInsets.Left * (_columnsCount + 1);
            var availableWidth = collectionView.Frame.Width - paddingSpace;
            var widthPerItem = availableWidth / _columnsCount;

            return new CGSize(width: widthPerItem, height: widthPerItem);
        }

        public override UIEdgeInsets GetInsetForSection(
            UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return _sectionInsets;
        }

        public override nfloat GetMinimumLineSpacingForSection(
            UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return _sectionInsets.Left;
        }

        // Enable Header
        public override CGSize GetReferenceSizeForHeader(
            UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new CGSize(width: collectionView.Frame.Width, height: 40);
        }

        // Enable Footer
        public override CGSize GetReferenceSizeForFooter(
            UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new CGSize(width: collectionView.Frame.Width, height: 20);
        }
    }
}
