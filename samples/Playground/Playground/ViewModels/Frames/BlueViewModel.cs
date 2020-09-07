// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class BlueViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public BlueViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public void GoNext()
        {
            _frameNavigationService.For<SplitFrameViewModel>().Navigate();
        }
    }
}
