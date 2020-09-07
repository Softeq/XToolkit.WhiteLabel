// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Frames
{
    public class TopShellViewModel : RootFrameNavigationPageViewModel<RedViewModel>
    {
        public TopShellViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }
    }
}
