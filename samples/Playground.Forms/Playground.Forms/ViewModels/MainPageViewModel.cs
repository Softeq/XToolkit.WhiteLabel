// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
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

        private string? _title;

        public MainPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            SimpleNavigationCommand = new RelayCommand(PerformSimpleNavigation);
            MasterDetailNavigationCommand = new RelayCommand(PerformMasterDetailNavigation);
        }

        public ICommand SimpleNavigationCommand { get; }

        public ICommand MasterDetailNavigationCommand { get; }

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private void PerformSimpleNavigation()
        {
            _pageNavigationService
                .For<FirstLevelViewModel>()
                .Navigate();
        }

        private void PerformMasterDetailNavigation()
        {
            _pageNavigationService
                .For<RootMasterViewModel>()
                .Navigate(true);
        }
    }
}
