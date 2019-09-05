// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.iOS.Views;
using Playground.ViewModels;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

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

            InitNavigationBar();
            InitTableView();
        }

        private void InitNavigationBar()
        {
            var navigationItem = NavigationController.NavigationBar.TopItem;

            navigationItem.Title = ViewModel.Title;
            navigationItem.SetCommand(UIBarButtonSystemItem.Compose, ViewModel.GoToEmptyCommand, false);
        }

        private void InitTableView()
        {
            TableView.RegisterNibForCellReuse(MainPageItemViewCell.Nib, MainPageItemViewCell.Key);

            // Use simple table

            //var source = new BindableTableViewSource<CommandAction, MainPageItemViewCell>(
            //    ViewModel.Items.Values.ToList());

            // Use group table
            TableView.RegisterNibForHeaderFooterViewReuse(
                MainPageGroupHeaderViewCell.Nib,
                MainPageGroupHeaderViewCell.Key);

            var source = new BindableGroupTableViewSource<string, CommandAction,
                MainPageGroupHeaderViewCell, MainPageItemViewCell>(
                TableView,
                ViewModel.Items)
            {
                HeightForRow = 60f,
                HeightForHeader = 100f,
            };

            source.ItemSelected += (sender, args) => args.Value.Command.Execute(null);

            TableView.Source = source;
        }
    }
}