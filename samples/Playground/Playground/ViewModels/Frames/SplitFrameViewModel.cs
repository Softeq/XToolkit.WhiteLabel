// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Messenger;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class SplitFrameViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public SplitFrameViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            TopShellViewModel = Dependencies.Container.Resolve<TopShellViewModel>();
        }

        public TopShellViewModel TopShellViewModel { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Messenger.Default.Register<GoBackMessage>(this, OnGoBackMessage);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            Messenger.Default.Unregister(this);
        }

        private void OnGoBackMessage(GoBackMessage message)
        {
            _frameNavigationService.GoBack();
        }
    }

    public class GoBackMessage
    {
    }
}
