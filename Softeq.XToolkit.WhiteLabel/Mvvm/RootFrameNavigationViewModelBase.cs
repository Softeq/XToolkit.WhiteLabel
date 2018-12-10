// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public abstract class RootFrameNavigationViewModelBase : ViewModelBase
    {
        private bool _canGoBack;

        protected RootFrameNavigationViewModelBase(IFrameNavigationService frameNavigationService)
        {
            FrameNavigationService = frameNavigationService;
        }

        public bool CanGoBack
        {
            get => _canGoBack;
            set => Set(ref _canGoBack, value);
        }

        public new bool IsInitialized => FrameNavigationService.IsInitialized;

        public abstract void NavigateToFirstPage();

        public override void OnInitialize()
        {
            base.OnInitialize();
            CanGoBack = FrameNavigationService.CanGoBack;
        }

        public void InitializeNavigation(object navigation)
        {
            FrameNavigationService.Initialize(navigation);
        }

        public void RestoreState()
        {
            if(FrameNavigationService.CanGoBack)
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