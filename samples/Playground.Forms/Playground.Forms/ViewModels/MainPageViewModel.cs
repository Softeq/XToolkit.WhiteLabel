// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Forms.ViewModels.Components;
using Playground.Forms.ViewModels.Dialogs;
using Playground.Forms.ViewModels.MasterDetailNavigation;
using Playground.Forms.ViewModels.SimpleNavigation;
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

            SimpleNavigationCommand = new RelayCommand(PerformSimpleNavigation);
            MasterDetailNavigationCommand = new RelayCommand(PerformMasterDetailNavigation);
            DialogsCommand = new RelayCommand(Dialogs);
            AsyncCommandsCommand = new RelayCommand(AsyncCommands);
            PermissionsCommand = new RelayCommand(Permissions);
        }

        public ICommand SimpleNavigationCommand { get; }

        public ICommand MasterDetailNavigationCommand { get; }

        public ICommand DialogsCommand { get; }

        public ICommand AsyncCommandsCommand { get; }

        public ICommand PermissionsCommand { get; }

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
                .For<DialogsRootPageViewModel>();
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
    }
}
