// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/>.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        ///     Sorts the elements in the current <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/>
        ///     using the provided comparer.
        /// </summary>
        /// <param name="collection">Collection to sort.</param>
        /// <param name="comparer">Custom comparer.</param>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> and <paramref name="comparer"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        public static void Sort<T>(this ObservableCollection<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            for (var i = 0; i < collection.Count; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < collection.Count; j++)
                {
                    if (comparer.Compare(collection[j], collection[minIndex]) < 0)
                    {
                        minIndex = j;
                    }
                }

                collection.Move(minIndex, i);
            }
        }

        /// <summary>
        ///     Sorts the current <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/>
        ///     using the provided method to compare the elements.
        /// </summary>
        /// <param name="collection">Collection to sort.</param>
        /// <param name="comparison">Method that compares <typeparamref name="T" /> objects.</param>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> and <paramref name="comparison"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var comparer = Comparer<T>.Create(comparison);
            collection.Sort(comparer);
        }

        /// <summary>
        ///     Sorts the current <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/>
        ///     using the default comparer.
        /// </summary>
        /// <param name="collection">Collection to sort.</param>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
        {
            collection.Sort(Comparer<T>.Default);
        }
    }
}
