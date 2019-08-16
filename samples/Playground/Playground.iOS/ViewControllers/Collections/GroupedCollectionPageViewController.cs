// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.Converters;
using Playground.iOS.Views.Collections;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class GroupedCollectionPageViewController : ViewControllerBase<GroupedCollectionPageViewModel>
    {
        public GroupedCollectionPageViewController(IntPtr handle) : base(handle)
        {
        }

        ~GroupedCollectionPageViewController()
        {
            Console.WriteLine($"Finalized: {nameof(GroupedCollectionPageViewController)}");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitCollectionView();
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ProductBasketViewModel.Status, () => Title);
            this.Bind(() => ViewModel.ProductListViewModel.IsBusy, () => ActivityIndicatorView.Hidden,
                new InverseBooleanConverter());
        }

        private void InitCollectionView()
        {
            // setup CollectionView
            CollectionView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            // pin headers
            ((UICollectionViewFlowLayout) CollectionView.CollectionViewLayout).SectionHeadersPinToVisibleBounds = true;

            // register footer view
            CollectionView.RegisterClassForSupplementaryView(
                typeof(GroupedFooterView),
                UICollectionElementKindSection.Footer,
                nameof(GroupedFooterView));

            // register header cell
            CollectionView.RegisterNibForSupplementaryView(
                GroupedHeaderView.Nib,
                UICollectionElementKindSection.Header,
                GroupedHeaderView.Key);

            // register item cell
            CollectionView.RegisterNibForCell(ProductViewCell.Nib, ProductViewCell.Key);

            // set custom delegate
            CollectionView.Delegate = new GroupedCollectionViewDelegateFlowLayout(columnsCount: 3);

            // set custom data source
            CollectionView.DataSource = new ProductsDataSource(ViewModel.ProductListViewModel.Products)
            {
                // main way for handle click by item
                ItemClick = ViewModel.AddToCartCommand
            };
        }

        private class ProductsDataSource : BindableGroupCollectionViewSource<
            ProductViewModel,       // item data type
            ProductViewCell,        // item cell type
            ProductHeaderViewModel, // header data type
            GroupedHeaderView>      // header view type
        {
            public ProductsDataSource(
                ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> items)
                : base(items)
            {
            }

            // for disable footer, see Delegate.GetReferenceSizeForFooter implementation
            protected override UICollectionReusableView GetFooterView(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var footer = collectionView.DequeueReusableSupplementaryView(
                    UICollectionElementKindSectionKey.Footer,
                    typeof(GroupedFooterView).Name,
                    indexPath);

                var bindableFooter = (IBindableView) footer;
                bindableFooter.ReloadDataContext($"Custom DataContext for section: {indexPath.Section}");

                return footer;
            }
        }
    }
}
