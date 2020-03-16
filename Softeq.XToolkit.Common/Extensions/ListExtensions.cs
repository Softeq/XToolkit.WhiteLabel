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
        public static void AddRange<T>(this IList<T> items, IList<T> range)
        {
            for (var i = 0; i < range.Count; i++)
            {
                items.Add(range[i]);
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
        public static void InsertRange<T>(this IList<T> items, int index, IList<T> range)
        {
            int i = index;
            foreach (var item in range)
            {
                items.Insert(i, item);
                i++;
            }
        }
    }
}
