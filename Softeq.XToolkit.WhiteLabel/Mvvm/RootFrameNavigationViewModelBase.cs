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

        public IFrameNavigationService FrameNavigationService { get; }

        public new bool IsInitialized => FrameNavigationService.IsInitialized;

        public void InitializeNavigation(object navigation)
        {
            FrameNavigationService.Initialize(navigation);
        }

        public abstract void NavigateToFirstPage();

        public virtual void RestoreState()
        {
            // Check fast-backward nav
            if (FrameNavigationService.CanGoBack)
            {
                FrameNavigationService.RestoreNavigation();
            }
            else
            {
                NavigateToFirstPage();
            }
        }
    }
}