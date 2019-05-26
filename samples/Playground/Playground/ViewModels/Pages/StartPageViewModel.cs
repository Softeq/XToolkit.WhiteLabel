// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public StartPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            _pageNavigationService.For<MainViewModel>().Navigate(true);
        }
    }
}
