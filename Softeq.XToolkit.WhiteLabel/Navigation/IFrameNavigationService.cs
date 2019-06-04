// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IFrameNavigationService
    {
        bool CanGoBack { get; }
        bool IsInitialized { get; }
        ViewModelBase CurrentViewModel { get; }

        void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>;

        void NavigateToViewModel(Type viewModelType, bool clearBackStack = false);
        void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase;
        void NavigateToViewModel<T>(T t) where T : IViewModelBase;
        void Initialize(object navigation);
        void RestoreState();
        void GoBack();
        void GoBack<T>() where T : ViewModelBase;

        void NavigateToViewModel<TViewModel>(IEnumerable<NavigationParameterModel> navigationParameters)
            where TViewModel : IViewModelBase;
    }
}