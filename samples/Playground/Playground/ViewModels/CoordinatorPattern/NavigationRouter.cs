using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.CoordinatorPattern
{
    public class NavigationRouter : IRouter
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public NavigationRouter(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
        }

        public void Present<TViewModel>(Action back, bool animated = true)
            where TViewModel : ViewModelBase
        {
            _frameNavigationService.For<TViewModel>()
                .Navigate();
        }

        public void Pop(bool animated = true)
        {
            _frameNavigationService.GoBack();
        }
    }
}
