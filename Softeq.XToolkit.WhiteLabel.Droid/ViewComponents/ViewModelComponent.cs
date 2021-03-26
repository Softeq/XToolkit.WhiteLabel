// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    /// <summary>
    ///     Component, which encapsulates logic of storing/restoring ViewModel instance within FragmentManager
    /// </summary>
    /// <typeparam name="TViewModel">
    ///     Type of the ViewModel instance
    /// </typeparam>
    public sealed class ViewModelComponent<TViewModel> where TViewModel : class
    {
        private TViewModel? _viewModel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelComponent{TViewModel}"/> class.
        /// </summary>
        /// <param name="viewModelKey">
        ///     Key identifier, which will be used for storing/restoring TViewModel instance.
        /// </param>
        public ViewModelComponent(string viewModelKey)
        {
            ViewModelKey = viewModelKey;
        }

        /// <summary>
        ///     Gets current instance of TViewModel, or throws exceptions if instance is not assigned.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        ///     ViewModel instance is not assigned either because it wasn't initialized using
        ///     <see cref="ViewModelComponent{TViewModel}.Initialize"/> method, or wasn't restored properly using
        ///     <see cref="ViewModelComponent{TViewModel}.RestoreViewModel"/> method.
        /// </exception>
        public TViewModel ViewModel => _viewModel ?? throw new InvalidOperationException("ViewModel instance is not assigned");

        /// <summary>
        ///     Gets key identifier witch used for storing/restoring TViewModel instance.
        /// </summary>
        public string ViewModelKey { get; }

        /// <summary>
        ///     Initializes <see cref="ViewModelComponent{TViewModel}"/> with an instance of ViewModel.
        /// </summary>
        /// <param name="viewModel">
        ///     ViewModel instance to store.
        /// </param>
        public void Initialize(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        /// <summary>
        ///     Saves current ViewModel instance within storage, associated with provided FragmentManager.
        /// </summary>
        /// <param name="fragmentManager">
        ///     Fragment manager where ViewModel should be stored.
        /// </param>
        public void SaveViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            viewModelStore.Add(ViewModelKey, ViewModel);
        }

        /// <summary>
        ///     Restores ViewModel instance from storage, associated with provided FragmentManager.
        /// </summary>
        /// <param name="fragmentManager">
        ///     Fragment manager where ViewModel has been stored.
        /// </param>
        public void RestoreViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            _viewModel = viewModelStore.Get<TViewModel>(ViewModelKey);
        }

        /// <summary>
        ///     Removes ViewModel instance from storage, associated with provided FragmentManager.
        /// </summary>
        /// <param name="fragmentManager">
        ///     Fragment manager from where ViewModel should be removed.
        /// </param>
        public void ClearViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            viewModelStore.Remove(ViewModelKey);
        }
    }
}
