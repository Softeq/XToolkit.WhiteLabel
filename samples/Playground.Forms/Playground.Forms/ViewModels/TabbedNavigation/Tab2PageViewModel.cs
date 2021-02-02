// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class Tab2PageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IFrameNavigationService _frameNavigationService;

        public Tab2PageViewModel(
            IPageNavigationService pageNavigationService,
            IFrameNavigationService frameNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _frameNavigationService = frameNavigationService;

            ToMainPageCommand = new RelayCommand(ToMainPage);
            ToNextPageCommand = new RelayCommand(ToNextPage);
        }

        public string Title => "Tab page 2";

        public ICommand ToMainPageCommand { get; }
        public ICommand ToNextPageCommand { get; }

        private void ToMainPage()
        {
            _pageNavigationService
                .For<MainPageViewModel>()
                .NavigateAsync(true);
        }

        private void ToNextPage()
        {
            _frameNavigationService
                .For<TabSubPageViewModel>()
                .From(this)
                .NavigateAsync();
        }
    }
}
