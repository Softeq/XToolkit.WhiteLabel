// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class TabSubPageViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public TabSubPageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public string Title => "Sub page";

    }
}
