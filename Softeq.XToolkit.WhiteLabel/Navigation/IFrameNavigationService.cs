// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IFrameNavigationService
    {
        bool IsInitialized { get; }
        bool IsEmptyBackStack { get; }
        bool CanGoBack { get; }

        void Initialize(object navigation);
        void GoBack();
        void GoBack<T>() where T : IViewModelBase;


        [Obsolete("Use .For<T> method instead.")]
        void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>;

        [Obsolete("Use .For<T> method instead.")]
        void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase;

        [Obsolete("Use .For<T> method instead.")]
        void NavigateToViewModel<T>(T t) where T : IViewModelBase;

        void NavigateToViewModel(Type viewModelType, bool clearBackStack = false);

        void NavigateToViewModel<TViewModel>(IEnumerable<NavigationParameterModel> navigationParameters)
            where TViewModel : IViewModelBase;


        /// <summary>
        ///     Tab was open first. (RootFrameViewModel)
        /// </summary>
        void NavigateToFirstPage();

        /// <summary>
        ///     Restore fragment after switch between tabs or back navigation.
        /// </summary>
        void RestoreState();
    }
}
