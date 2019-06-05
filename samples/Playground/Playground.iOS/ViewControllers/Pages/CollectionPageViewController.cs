// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Playground.iOS.Views;
using Playground.Models;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class CollectionPageViewController
        : ViewControllerBase<CollectionPageViewModel>
    {
        private UIColor _navBarColor;
        public CollectionPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CollectionView.RegisterNibForCell(MovieCollectionViewCell.Nib, MovieCollectionViewCell.Key);
            CollectionView.DataSource = new CustomCollectionViewSource(ViewModel.Items);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var navBar = NavigationController.NavigationBar;

            Title = "Movies";

            _navBarColor = navBar.BarTintColor;

            navBar.TitleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.White,
            };

            navBar.BarTintColor = UIColor.FromRGB(28, 28, 28);
            navBar.Translucent = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            NavigationController.NavigationBar.BarTintColor = _navBarColor;
            base.ViewWillDisappear(animated);
        }

        private class CustomCollectionViewSource : BindableCollectionViewSource<ItemModel>
        {
            private readonly List<ItemModel> _items;

            public CustomCollectionViewSource(List<ItemModel> items)
            {
                _items = items;
                DataSource = _items;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return _items.Count;
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
            {
                return (UICollectionViewCell) collectionView.DequeueReusableCell(MovieCollectionViewCell.Key, indexPath);
            }
        }
    }
}
