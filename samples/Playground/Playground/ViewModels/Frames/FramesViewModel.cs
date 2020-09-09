// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class FramesViewModel : RootFrameNavigationPageViewModel<BlueViewModel>
    {
        public FramesViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public bool TryHandleBackPress()
        {
            if (FrameNavigationService.CanGoBack)
            {
                FrameNavigationService.GoBack();
                return false;
            }

            return true;
        }
    }
}
