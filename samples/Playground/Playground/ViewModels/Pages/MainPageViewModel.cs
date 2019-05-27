// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _navigationService;

        public MainPageViewModel(IPageNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void NavigateToCollectionsSample()
        {
            _navigationService.NavigateToViewModel<CollectionPageViewModel>();
        }
    }
}
