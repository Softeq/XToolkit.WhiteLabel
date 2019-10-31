// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.Tab
{
    public class TabNavigationService : ITabNavigationService
    {
        public IFrameNavigationService CurrentFrameNavigationService { get; private set; } = default!;

        public bool CanGoBack => CurrentFrameNavigationService.CanGoBack;

        public void GoBack()
        {
            CurrentFrameNavigationService.GoBack();
        }

        public void SetSelectedViewModel(ViewModelBase selectedViewModel)
        {
            CurrentFrameNavigationService = selectedViewModel.FrameNavigationService;
        }
    }
}
