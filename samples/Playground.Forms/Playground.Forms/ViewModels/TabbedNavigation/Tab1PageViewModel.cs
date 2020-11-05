// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class Tab1PageViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IPageNavigationService _pageNavigationService;

        public Tab1PageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            ToMainPageCommand = new RelayCommand(ToMainPage);
            ToNextPageCommand = new RelayCommand(ToNextPage);
        }

        public string? Title => "Page 1";
        public string? IconImageSource => "AppIcon";

        public ICommand ToMainPageCommand { get; }
        public ICommand ToNextPageCommand { get; }

        private void ToMainPage()
        {
            _pageNavigationService
                .For<MainPageViewModel>()
                .Navigate(true);
        }

        private void ToNextPage()
        {
            _pageNavigationService
              .For<SecondPageViewModel>()
              .WithParam(x => x.NavigationParameter, "Custom Parameter")
              .Navigate();
        }
    }
}
