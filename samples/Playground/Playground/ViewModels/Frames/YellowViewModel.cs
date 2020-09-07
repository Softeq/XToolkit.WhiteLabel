// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class YellowViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public YellowViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public void GoBack()
        {
            _frameNavigationService.GoBack();
        }
    }
}
