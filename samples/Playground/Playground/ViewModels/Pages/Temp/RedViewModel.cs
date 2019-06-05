using System;
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
            _pageNavigationService = pageNavigationService;
            NavigateCommand = new RelayCommand(HandleAction);
        }

        public ICommand NavigateCommand { get; }

        private void HandleAction()
        {
            FrameNavigationService.For<YellowViewModel>().Navigate();
        }
    }
}
