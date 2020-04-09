// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class RootFrameNavigationPageViewModel<T> : RootFrameNavigationViewModelBase where T : IViewModelBase
    {
        public RootFrameNavigationPageViewModel(IFrameNavigationService frameNavigationService) : base(frameNavigationService) { }

        public override void NavigateToFirstPage()
        {
            FrameNavigationService
                .For<T>()
                .Navigate(true);
        }
    }
}
