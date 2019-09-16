// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Playground.iOS.Views.Table;
using Playground.ViewModels.Collections;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
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

            AddButton.SetCommand(ViewModel.AddAllToCartCommand);
            GenerateButton.SetCommand(ViewModel.GenerateGroupCommand);

            TableView.RegisterNibForCellReuse(ProductTableViewCell.Nib, ProductTableViewCell.Key);
            TableView.RegisterNibForHeaderFooterViewReuse(GroupedTableHeaderView.Nib, GroupedTableHeaderView.Key);
            TableView.RegisterClassForHeaderFooterViewReuse(typeof(GroupedTableFooterView), nameof(GroupedTableFooterView));

            TableView.Source = new CustomSource(TableView, ViewModel.ProductListViewModel.Products)
            {
                HeightForRow = 60f,
                HeightForHeader = 40f,
                HeightForFooter = 20f
            };
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ProductBasketViewModel.Status, () => Title);

            //this.Bind(() => ViewModel.ProductListViewModel.IsBusy, () => BusyView.Hidden,
            //    new InverseBooleanConverter());
        }

        private class CustomSource : BindableGroupTableViewSource<ProductHeaderViewModel,
                ProductViewModel,
                GroupedTableHeaderView,
                GroupedTableFooterView,
                ProductTableViewCell>
        {
            public CustomSource(UITableView tableView, IEnumerable<IGrouping<ProductHeaderViewModel, ProductViewModel>> items)
                : base(tableView, items)
            {
            }
        }
    }
}
