// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class RedViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public RedViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public void GoNext()
        {
            _frameNavigationService.For<YellowViewModel>().Navigate();
        }
    }
}
