// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    internal class GroupedCollectionViewDelegateFlowLayout : BindableUICollectionViewDelegateFlowLayout
    {
        private readonly int _itemsPerRow = 3;

        private UIEdgeInsets _sectionInsets = new UIEdgeInsets(top: 20, left: 20, bottom: 20, right: 20);

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var paddingSpace = _sectionInsets.Left * (_itemsPerRow + 1);
            var availableWidth = collectionView.Frame.Width - paddingSpace;
            var widthPerItem = availableWidth / _itemsPerRow;

            return new CGSize(width: widthPerItem, height: widthPerItem);
        }

        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return _sectionInsets;
        }

        public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return _sectionInsets.Left;
        }

        // Enable Header
        public override CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new CGSize(width: collectionView.Frame.Width, height: 40);
        }

        // Enable Footer
        public override CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new CGSize(width: collectionView.Frame.Width, height: 20);
        }
    }
}
