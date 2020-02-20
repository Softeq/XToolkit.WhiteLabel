// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        private string _title;

        public MainPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            GoNextCommand = new RelayCommand(NavigateToSecondPage);
        }

        public ICommand GoNextCommand { get; }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private void NavigateToSecondPage()
        {
            _pageNavigationService
                .For<SecondPageViewModel>()
                .Navigate();
        }
    }
}
