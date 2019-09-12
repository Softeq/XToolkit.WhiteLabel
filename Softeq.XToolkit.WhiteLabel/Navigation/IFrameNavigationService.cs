// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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

        /// <summary>
        ///     Use <see cref="FrameNavigationServiceExtensions.For{T}(IFrameNavigationService)"/> instead.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model.</typeparam>
        /// <param name="clearBackStack"></param>
        /// <param name="parameters"></param>
        void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null)
            where TViewModel : IViewModelBase;

        void NavigateToViewModel(
            Type viewModelType,
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null);

        /// <summary>
        ///     Navigates to the first page of the navigation stack.
        /// </summary>
        void NavigateToFirstPage();

        /// <summary>
        ///     Restores the last fragment after a switch between tabs or back navigation.
        /// </summary>
        void RestoreNavigation();
    }
}
