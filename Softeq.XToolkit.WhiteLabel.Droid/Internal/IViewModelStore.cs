// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    /// <summary>
    ///     Defines the contract for a store that keeps view models.
    /// </summary>
    internal interface IViewModelStore
    {
        /// <summary>
        ///     Returns an existing <typeparamref name="TViewModel"/> instance from the store.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model to get.</typeparam>
        /// <param name="key">The view model unique key.</param>
        /// <returns>The view model instance.</returns>
        TViewModel Get<TViewModel>(string key) where TViewModel : class, IViewModelBase;

        /// <summary>
        ///     Adds a <typeparamref name="TViewModel"/> instance to the store by <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model to add.</typeparam>
        /// <param name="key">The view model unique key.</param>
        /// <param name="viewModel">The view model to add.</param>
        void Add(string key, IViewModelBase viewModel);

        /// <summary>
        ///     Removes an existing view model instance from the store by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The view model unique key.</param>
        void Remove(string key);

        /// <summary>
        ///     Clears store.
        /// </summary>
        void Clear();
    }
}
