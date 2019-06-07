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
        private int _count;

        public RedViewModel()
        {
            NavigateCommand = new RelayCommand(HandleAction);
            IncrementCommand = new RelayCommand(Increment);
        }

        public ICommand NavigateCommand { get; }

        public ICommand IncrementCommand { get; }

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        private void HandleAction()
        {
            FrameNavigationService.For<YellowViewModel>().Navigate();
        }

        private void Increment()
        {
            Count++;
        }
    }
}
