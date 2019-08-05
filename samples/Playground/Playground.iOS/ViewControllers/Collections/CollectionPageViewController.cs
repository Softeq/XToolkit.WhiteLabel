// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Playground.iOS.Views;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
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
            CollectionView.DataSource = new CustomCollectionViewSource(ViewModel.ItemModels);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            InitNavBar();
        }

        public override void ViewWillDisappear(bool animated)
        {
            NavigationController.NavigationBar.BarTintColor = _navBarColor;

            base.ViewWillDisappear(animated);
        }

        private void InitNavBar()
        {
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
        }
    }
}
