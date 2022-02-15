// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.Forms.Remote.ViewModels;
using Playground.Forms.ViewModels.Components;
using Playground.Forms.ViewModels.Dialogs;
using Playground.Forms.ViewModels.MasterDetailNavigation;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        private string _title = string.Empty;

        public MainPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            Items = new ObservableRangeCollection<CommandAction>(
                new List<CommandAction>
                {
                    new CommandAction(new RelayCommand(PerformSimpleNavigation), "Simple Navigation"),
                    new CommandAction(new RelayCommand(PerformMasterDetailNavigation), "Master Detail Navigation"),
                    new CommandAction(new RelayCommand(Dialogs), "Dialogs"),
                    new CommandAction(new RelayCommand(AsyncCommands), "Async Commands"),
                    new CommandAction(new RelayCommand(Permissions), "Permissions"),
                    new CommandAction(new RelayCommand(Validation), "Validation"),
                    new CommandAction(new RelayCommand(PaginationSearch), "Pagination Search"),
                    new CommandAction(new RelayCommand(Remote), "Remote"),
                    new CommandAction(new RelayCommand(Launcher), "Launcher"),
                    new CommandAction(new RelayCommand(Location), "Location"),
                });
        }

        public ObservableRangeCollection<CommandAction> Items { get; }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private void PerformSimpleNavigation()
        {
            _pageNavigationService
                .For<FirstPageViewModel>()
                .Navigate();
        }

        private void PerformMasterDetailNavigation()
        {
            _pageNavigationService
                .For<RootMasterDetailPageViewModel>()
                .Navigate(true);
        }

        private void Dialogs()
        {
            _pageNavigationService
                .For<DialogsRootPageViewModel>()
                .Navigate();
        }

        private void AsyncCommands()
        {
            _pageNavigationService
                .For<AsyncCommandsPageViewModel>()
                .Navigate();
        }

        private void Permissions()
        {
            _pageNavigationService
                .For<PermissionsPageViewModel>()
                .Navigate();
        }

        private void Validation()
        {
            _pageNavigationService
                .For<ValidationPageViewModel>()
                .Navigate();
        }

        private void PaginationSearch()
        {
            _pageNavigationService
                .For<PaginationSearchPageViewModel>()
                .Navigate();
        }

        private void Remote()
        {
            _pageNavigationService
                .For<RemotePageViewModel>()
                .Navigate();
        }

        private void Launcher()
        {
            _pageNavigationService
                .For<LauncherPageViewModel>()
                .Navigate();
        }

        private void Location()
        {
            _pageNavigationService
                .For<LocationPageViewModel>()
                .Navigate();
        }
    }
}
