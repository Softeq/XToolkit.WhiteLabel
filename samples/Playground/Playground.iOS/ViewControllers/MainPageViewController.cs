// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Playground.iOS.Views;
using Playground.ViewModels;

namespace Playground.iOS.ViewControllers
{
    public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
    {
        public MainPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitTableView();
        }

        private void InitTableView()
        {
            TableView.RegisterNibForCellReuse(MainPageItemViewCell.Nib, MainPageItemViewCell.Key);
            TableView.RegisterNibForHeaderFooterViewReuse(MainPageGroupHeaderViewCell.Nib,
                MainPageGroupHeaderViewCell.Key);

            var source = new ObservableGroupTableViewSource<string, CommandAction>(
                TableView,
                ViewModel.Items,
                (tableView, item, dataSource, indexPath) =>
                {
                    var cell = tableView.DequeueReusableCell(MainPageItemViewCell.Key, indexPath);
                    var itemCell = (MainPageItemViewCell) cell;
                    itemCell.SetDataContext(item);
                    return itemCell;
                },
                getHeaderViewFunc: (tableView, headerKey) =>
                {
                    var cell = tableView.DequeueReusableHeaderFooterView(MainPageGroupHeaderViewCell.Key);
                    var headerCell = (MainPageGroupHeaderViewCell) cell;
                    headerCell.SetDataContext(headerKey);
                    return headerCell;
                })
            {
                HeightForRow = 60f,
                HeightForHeader = 100f
            };

            source.ItemSelected += (sender, args) => args.Value.Command.Execute(null);

            TableView.Source = source;
        }
    }
}