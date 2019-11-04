// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.BottomTabs.First
{
    public class RedViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        private int _count;

        public RedViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            NavigateCommand = new RelayCommand(Navigate);
            IncrementCommand = new RelayCommand(Increment);
        }

        public ICommand NavigateCommand { get; }

        public ICommand IncrementCommand { get; }

        public int Count
        {
            get => _count;
            private set => Set(ref _count, value);
        }

        private void Navigate()
        {
            _pageNavigationService.For<YellowViewModel>().Navigate();
        }

        private void Increment()
        {
            Count++;
        }
    }
}
