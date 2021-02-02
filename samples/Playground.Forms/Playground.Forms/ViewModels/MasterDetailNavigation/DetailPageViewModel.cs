// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public class DetailPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public DetailPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            ToMainPageCommand = new RelayCommand(ToMainPage);
        }

        public ICommand ToMainPageCommand { get; }

        private void ToMainPage()
        {
            _pageNavigationService
                .For<MainPageViewModel>()
                .NavigateAsync(true);
        }
    }
}
