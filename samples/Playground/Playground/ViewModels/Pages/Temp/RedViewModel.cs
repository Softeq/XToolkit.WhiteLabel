// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages.Temp
{
    public class RedViewModel : ViewModelBase
    {
        public RedViewModel()
        {
            NavigateCommand = new RelayCommand(HandleAction);
        }

        public ICommand NavigateCommand { get; }

        private void HandleAction()
        {
            FrameNavigationService.For<YellowViewModel>().Navigate();
        }
    }
}
