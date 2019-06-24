﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.Tab
{
    public interface ITabNavigationService
    {
        bool CanGoBack { get; }
        IFrameNavigationService CurrentFrameNavigationService { get; }
        void GoBack();
        void SetSelectedViewModel(ViewModelBase selectedViewModel);
    }
}