// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    /// <summary>
    ///     Defines the contract for a storage that stores instances of different classes.
    /// </summary>
    internal interface IInstanceStorage
    {
        /// <summary>
        ///     Returns an existing <typeparamref name="T"/> instance from the store.
        /// </summary>
        /// <typeparam name="T">The type of the instance to get.</typeparam>
        /// <param name="key">The instance unique key.</param>
        /// <returns>Stored instance.</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        ///     Adds a <typeparamref name="T"/> instance to the store by <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance to add.</typeparam>
        /// <param name="key">The instance unique key.</param>
        /// <param name="viewModel">The instance to add.</param>
        void Add<T>(string key, T viewModel) where T : class;

        /// <summary>
        ///     Removes an existing instance from the store by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The instance unique key.</param>
        void Remove(string key);

        /// <summary>
        ///     Removes an existing instances from the store by <paramref name="keys"/>.
        /// </summary>
        /// <param name="keys">The collection of instance unique keys.</param>
        void Remove(IEnumerable<string> keys);

        /// <summary>
        ///     Clears store.
        /// </summary>
        void Clear();
    }
}
