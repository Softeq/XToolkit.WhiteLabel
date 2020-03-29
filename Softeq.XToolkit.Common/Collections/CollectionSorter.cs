// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Softeq.XToolkit.Common.Collections
{
    public static class CollectionSorter
    {
        /// <summary>
        ///     Sort the specified collection in ascending order.
        /// </summary>
        /// <param name="collection">Collection to sort.</param>
        /// <param name="comparison">Method that compares <typeparamref name="T" /> objects</param>
        /// <typeparam name="T">The element type of the collection</typeparam>
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var comparer = new Comparer<T>(comparison);

            var sorted = collection.OrderBy(x => x, comparer).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        /// <summary>
        ///     Sort the specified collection in descending order.
        /// </summary>
        /// <param name="collection">Collection to sort.</param>
        /// <param name="comparison">Method that compares <typeparamref name="T" /> objects</param>
        /// <typeparam name="T">The element type of the collection</typeparam>
        public static void DescendingSort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var comparer = new ReverseComparer<T>(comparison);

            var sorted = collection.OrderBy(x => x, comparer).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }
    }

    internal class Comparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Collections.Comparer`1" /> class.
        /// </summary>
        /// <param name="comparison">Method that compares <typeparamref name="T" /> objects</param>
        public Comparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return _comparison.Invoke(x, y);
        }

        #endregion
    }

    internal class ReverseComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Collections.ReverseComparer`1" /> class.
        /// </summary>
        /// <param name="comparison">Method that compares <typeparamref name="T" /> objects</param>
        public ReverseComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return -_comparison.Invoke(x, y);
        }

        #endregion
    }
}
