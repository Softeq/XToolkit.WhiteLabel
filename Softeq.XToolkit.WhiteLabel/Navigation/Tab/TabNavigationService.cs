using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.Tab
{
    public class TabNavigationService : ITabNavigationService
    {
        private IFrameNavigationService _currentFrameNavigationService;

        public bool CanGoBack => _currentFrameNavigationService.CanGoBack;

        public void GoBack()
        {
            _currentFrameNavigationService.GoBack();
        }

        public void SetSelectedViewModel(ViewModelBase selectedViewModel)
        {
            _currentFrameNavigationService = selectedViewModel.FrameNavigationService;
        }
    }
}
