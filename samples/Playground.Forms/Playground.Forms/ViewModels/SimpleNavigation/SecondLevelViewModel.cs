// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.SimpleNavigation
{
    public class SecondLevelViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public SecondLevelViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            BackCommand = new RelayCommand(Back);
        }

        public ICommand BackCommand { get; }

        public string? NavigationParameter { get; set; }

        public string ParameterText => $"parameter: {NavigationParameter}";

        private void Back()
        {
            _pageNavigationService.GoBack();
        }
    }
}
