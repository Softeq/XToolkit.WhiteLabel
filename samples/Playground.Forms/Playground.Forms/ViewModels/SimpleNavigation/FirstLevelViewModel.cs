// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.SimpleNavigation
{
    public class FirstLevelViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;
        private string? _text;

        public FirstLevelViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            NextLevelCommand = new RelayCommand(NextLevel);
        }

        public ICommand NextLevelCommand { get; }

        public string? Text
        {
            get => _text;
            set => Set(ref _text, value);
        }

        private void NextLevel()
        {
            _pageNavigationService
                .For<SecondLevelViewModel>()
                .WithParam(x => x.NavigationParameter, Text)
                .Navigate();
        }
    }
}
