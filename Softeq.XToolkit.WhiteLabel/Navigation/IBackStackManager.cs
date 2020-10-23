// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     An interface that contains methods for managing ViewModels back stack.
    /// </summary>
    public interface IBackStackManager
    {
        /// <summary>
        ///     Gets the number of items in back stack.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Adds the specified ViewModel to the end of back stack.
        /// </summary>
        /// <param name="viewModel">ViewModel to add.</param>
        void PushViewModel(IViewModelBase viewModel);

        /// <summary>
        ///     Removes the last added ViewModel from back stack.
        /// </summary>
        /// <returns> Removed ViewModel.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Back stack is empty.
        /// </exception>
        IViewModelBase PopViewModel();

        /// <summary>
        ///     Returns a value that indicates whether there is a ViewModel at the top of back stack,
        ///     and if one is present, copies it to the result parameter and removes it from back stack.
        /// </summary>
        /// <param name="result">
        ///     ViewModel at the top of back stack if it is not empty;
        ///     otherwise, <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if back stack is not empty; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryPopViewModel(out IViewModelBase result);

        /// <summary>
        ///     Returns the last added ViewModel without removing it.
        /// </summary>
        /// <returns>Last added ViewModel.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Back stack is empty.
        /// </exception>
        IViewModelBase PeekViewModel();

        /// <summary>
        ///     Returns a value that indicates whether there is a ViewModel at the top of back stack,
        ///     and if one is present, copies it to the result parameter.
        ///     ViewModel is not removed from back stack.
        /// </summary>
        /// <param name="result">
        ///     ViewModel at the top of back stack if the stack is not empty;
        ///     otherwise, <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the stack is not empty; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryPeekViewModel(out IViewModelBase result);

        /// <summary>
        ///     Clears back stack.
        /// </summary>
        void Clear();
    }
}