using System;
using Playground.iOS.Views;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class TablePageViewController : ViewControllerBase<TablePageViewModel>
    {
        public TablePageViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.RegisterNibForCellReuse(MovieTableViewCell.Nib, MovieTableViewCell.Key);
            TableView.Source = new BindableTableViewSource<ItemViewModel, MovieTableViewCell>(TableView, ViewModel.ItemModels)
            {
                HeightForRow = 60,
                ItemClick = ViewModel.SelectItemCommand
            };
        }
    }
}

