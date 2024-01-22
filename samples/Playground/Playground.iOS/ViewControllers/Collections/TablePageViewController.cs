// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.iOS.Views;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class TablePageViewController : ViewControllerBase<TablePageViewModel>
    {
        public TablePageViewController(NativeHandle handle)
            : base(handle)
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
