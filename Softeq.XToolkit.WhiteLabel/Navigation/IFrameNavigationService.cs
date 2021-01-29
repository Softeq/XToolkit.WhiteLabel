// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IFrameNavigationService
    {
        /// <summary>
        ///     Gets a value indicating whether this service is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Gets a value indicating whether the backstack is empty or not.
        /// </summary>
        bool IsEmptyBackStack { get; }

        /// <summary>
        ///     Gets a value indicating whether we can go back to the previous frame from the current state.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        ///     Initializes the current service with the specified navigation object.
        /// </summary>
        /// <param name="navigation">
        ///     Navigation object (usually platform-specific) that is used for initialization.
        /// </param>
        void Initialize(object navigation);

        /// <summary>
        ///     Goes back to the previous frame if it is possible.
        /// </summary>
        void GoBack();

        /// <summary>
        ///     Goes back to the previous frame untill the current ViewModel is not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"> Type of the ViewModel that we're looking for.</typeparam>
        void GoBack<T>() where T : IViewModelBase;

        /// <summary>
        ///     Use <see cref="FrameNavigationServiceExtensions.For{T}(IFrameNavigationService)" /> instead.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model.</typeparam>
        /// <param name="clearBackStack">The value indicating if we should clear backstack after navigating.</param>
        /// <param name="parameters">List of navigation parameters.</param>
        Task NavigateToViewModelAsync<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase;

        /// <summary>
        ///     Navigates to the first page of the navigation stack.
        /// </summary>
        Task NavigateToFirstPageAsync();

        /// <summary>
        ///     Restores the last fragment after a switch between tabs or back navigation.
        /// </summary>
        void RestoreNavigation();

        /// <summary>
        ///     Restores the last fragment only if there was an unfinished navigation transaction.
        /// </summary>
        void RestoreUnfinishedNavigation();
    }
}