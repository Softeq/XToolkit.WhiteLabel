// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Messenger;
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

            NextCommand = new RelayCommand(GoNext);
            BackCommand = new RelayCommand(GoBack);
        }

        public RelayCommand NextCommand { get; }

        public RelayCommand BackCommand { get; }

        public string NextText { get; } = "next top frame";

        public string BackText { get; } = "prev frame";

        private void GoNext()
        {
            _frameNavigationService.For<YellowViewModel>().NavigateAsync();
        }

        private void GoBack()
        {
            Messenger.Default.Send(new GoBackMessage());
        }
    }
}
