using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages.Temp
{
    public class BlueViewModel : ViewModelBase
    {
        public BlueViewModel()
        {
            NavigateCommand = new RelayCommand(HandleAction);
        }

        public ICommand NavigateCommand { get; }

        private void HandleAction()
        {
            FrameNavigationService.For<GreenViewModel>().Navigate();
        }
    }
}
