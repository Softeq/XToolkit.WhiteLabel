// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class Tab1PageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IFrameNavigationService _frameNavigationService;

        public Tab1PageViewModel(
            IPageNavigationService pageNavigationService,
            IFrameNavigationService frameNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _frameNavigationService = frameNavigationService;

            ToMainPageCommand = new AsyncCommand(ToMainPage);
            ToNextPageCommand = new AsyncCommand(ToNextPage);
        }

        public string Title => "Tab page 1";

        public ICommand ToMainPageCommand { get; }
        public ICommand ToNextPageCommand { get; }

        private async Task ToMainPage()
        {
            await _pageNavigationService
                .For<MainPageViewModel>()
                .NavigateAsync(true);
        }

        private async Task ToNextPage()
        {
            await _frameNavigationService
                .For<TabSubPageViewModel>()
                .From(this)
                .NavigateAsync();
        }
    }
}
