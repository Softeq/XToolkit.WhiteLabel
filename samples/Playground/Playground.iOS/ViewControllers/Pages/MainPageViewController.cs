// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.iOS.Views;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
    {
        public MainPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Main Page";

            TableView.RegisterNibForCellReuse(MainPageItemViewCell.Nib, MainPageItemViewCell.Key);
            TableView.RegisterNibForHeaderFooterViewReuse(MainPageGroupHeaderViewCell.Nib,
                MainPageGroupHeaderViewCell.Key);

            TableView.Source = new ObservableGroupTableViewSource<string, CommandAction>(
                TableView,
                ViewModel.Items,
                (tableView, item, dataSource, indexPath) =>
                {
                    var cell = (MainPageItemViewCell) tableView.DequeueReusableCell(MainPageItemViewCell.Key,
                        indexPath);
                    cell.BindCell(item);
                    return cell;
                },
                getHeaderViewFunc: (tableView, headerKey) =>
                {
                    var header =
                        (MainPageGroupHeaderViewCell) tableView.DequeueReusableHeaderFooterView(
                            MainPageGroupHeaderViewCell.Key);
                    header.BindCell(headerKey);
                    return header;
                })
            {
                HeightForRow = 60f,
                HeightForHeader = 100f
            };
            (TableView.Source as ObservableGroupTableViewSource<string, CommandAction>).ItemSelected +=
                (sender, args) => args.Value.Command.Execute(null);
        }
    }
}