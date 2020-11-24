// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class Tab1PageViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IFrameNavigationService _frameNavigationService;

        public Tab1PageViewModel(IPageNavigationService pageNavigationService, IFrameNavigationService frameNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _frameNavigationService = frameNavigationService;

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
            _frameNavigationService
              .For<FirstPageViewModel>()
              .From(this)
              .Navigate();
        }
    }
}
