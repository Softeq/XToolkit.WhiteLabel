// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class SettingsTabPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public SettingsTabPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            BackToTabsCommand = new RelayCommand(BackToTabs);
            BackToMainMenuCommand = new RelayCommand(BackToMainMenu);
        }

        public string Title { get; } = "Tab Settings";
        public ICommand BackToTabsCommand { get; }
        public ICommand BackToMainMenuCommand { get; }

        private void BackToTabs()
        {
            _pageNavigationService.GoBack();
        }

        private void BackToMainMenu()
        {
            _pageNavigationService
                .For<MainPageViewModel>()
                .Navigate(true);
        }
    }
}