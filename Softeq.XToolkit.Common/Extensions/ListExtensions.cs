// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Collections.Generic.IList`1" />
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        ///     Adds the elements of the specified collection to the end
        ///     of the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="items">Initial collection.</param>
        /// <param name="range">The collection whose elements should be added to the end of the list.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        public static void AddRange<T>(this IList<T> items, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                items.Add(item);
            }
        }

        /// <summary>
        ///     Inserts the elements of a collection into the <see cref="T:System.Collections.Generic.List`1" />
        ///     at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="items">Source collection.</param>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="range">The collection whose elements should be added to the end of the list.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        public static void InsertRange<T>(this IList<T> items, int index, IEnumerable<T> range)
        {
            int i = index;
            foreach (var item in range)
            {
                items.Insert(i, item);
                i++;
            }
        }

        /// <summary>
        ///     Adds the element to the end of the collection in a fluent manner.
        /// </summary>
        /// <param name="list">Initial collection.</param>
        /// <param name="item">Item to add.</param>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <returns>Initial collection with an added item.</returns>
        public static IList<T> AddItem<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }
}
