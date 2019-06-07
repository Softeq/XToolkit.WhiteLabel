// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages.Temp
{
    public class RedViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public RedViewModel(IPageNavigationService pageNavigationService)
        {
            NavigateCommand = new RelayCommand(HandleAction);
            _pageNavigationService = pageNavigationService;
        }

        public ICommand NavigateCommand { get; }

        private void HandleAction()
        {
            _pageNavigationService.For<CollectionPageViewModel>().Navigate();
            //FrameNavigationService.For<CollectionPageViewModel>().Navigate();
        }
    }
}
