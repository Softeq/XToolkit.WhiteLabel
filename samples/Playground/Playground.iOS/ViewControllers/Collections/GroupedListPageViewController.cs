using System;
using System.Collections.Generic;
using Foundation;
using Playground.iOS.Views.Table;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    [Register(nameof(GroupedListPageViewController))]
    public partial class GroupedListPageViewController : ViewControllerBase<GroupedTablePageViewModel>
    {
        public GroupedListPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.RegisterNibForCellReuse(ProductTableViewCell.Nib, ProductTableViewCell.Key);
            TableView.RegisterNibForHeaderFooterViewReuse(GroupedTableHeaderView.Nib, GroupedTableHeaderView.Key);
            //TableView.RegisterClassForCellReuse(typeof(GroupedTableFooterView), nameof(GroupedTableFooterView));

            TableView.Source = new ObservableGroupTableViewSourceNew<ProductHeaderViewModel, ProductViewModel>(
                TableView,
                ViewModel.ProductListViewModel.Products,
                GetCell,
                null,
                GetHeaderView,
                null,
                GetHeaderHeight,
                null,
                GetHeightForRow);
        }

        private UIView GetHeaderView(UITableView tableView, ProductHeaderViewModel viewModel)
        {
            var view = tableView.DequeueReusableHeaderFooterView(GroupedTableHeaderView.Key);

            ((IBindableView) view).SetDataContext(viewModel);
            ((IBindableView) view).DoDetachBindings();
            ((IBindableView) view).DoAttachBindings();

            return view;
        }

        private nfloat GetHeaderHeight(ProductHeaderViewModel viewModel)
        {
            return 40f;
        }

        private nfloat GetHeightForRow(NSIndexPath indexPath)
        {
            return 60;
        }

        private UITableViewCell GetCell(UITableView table, ProductViewModel viewModel, IList<ProductViewModel> viewModels, NSIndexPath indexPath)
        {
            var cell = table.DequeueReusableCell(ProductTableViewCell.Key, indexPath);

            ((IBindableView) cell).SetDataContext(viewModel);
            ((IBindableView) cell).DoDetachBindings();
            ((IBindableView) cell).DoAttachBindings();

            return cell;
        }
    }
}
