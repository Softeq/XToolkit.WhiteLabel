// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.BottomTabs.Second
{
    public class BlueViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;
        private int _count;

        public BlueViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;
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

        private void Increment()
        {
            Count++;
        }

        private void Navigate()
        {
            _frameNavigationService.For<GreenViewModel>().Navigate();
        }
    }
}