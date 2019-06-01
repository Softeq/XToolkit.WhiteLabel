// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public abstract class RootFrameNavigationViewModelBase : ViewModelBase
    {
        protected RootFrameNavigationViewModelBase(IFrameNavigationService frameNavigationService)
        {
            FrameNavigationService = frameNavigationService;
        }

        public new bool IsInitialized => FrameNavigationService.IsInitialized;

        public abstract void NavigateToFirstPage();

        public void InitializeNavigation(object navigation)
        {
            FrameNavigationService.Initialize(navigation);
        }

        public void RestoreState()
        {
            if (FrameNavigationService.CanGoBack)
            {
                FrameNavigationService.RestoreState();
            }
            else
            {
                NavigateToFirstPage();
            }
        }
    }
}