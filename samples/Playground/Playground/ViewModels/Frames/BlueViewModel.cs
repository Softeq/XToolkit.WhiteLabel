// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class BlueViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        public BlueViewModel(
            IFrameNavigationService frameNavigationService,
            IPageNavigationService pageNavigationService)
        {
            _frameNavigationService = frameNavigationService;
            _pageNavigationService = pageNavigationService;

            NextCommand = new RelayCommand(GoNext);
            BackCommand = new RelayCommand(GoBack);
        }

        public RelayCommand NextCommand { get; }

        public RelayCommand BackCommand { get; }

        public string NextText { get; } = "next frame";

        public string BackText { get; } = "prev page";

        private void GoNext()
        {
            _frameNavigationService.For<SplitFrameViewModel>().Navigate();
        }

        private void GoBack()
        {
            _pageNavigationService.GoBack();
        }
    }
}
