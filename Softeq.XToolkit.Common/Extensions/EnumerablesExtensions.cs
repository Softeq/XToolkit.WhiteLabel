// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Collections.Generic.IEnumerable`1" />
    /// </summary>
    public static class EnumerablesExtensions
    {
        /// <summary>
        ///     Returns empty IEnumerable if source is null.
        /// </summary>
        /// <returns>Return IEnumerable if source is null otherwise return source.</returns>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">Item type.</typeparam>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Apply specified action to each item of enumerable.
        /// </summary>
        /// <param name="enumerable">IEnumerable instance.</param>
        /// <param name="action">Specified action.</param>
        /// <typeparam name="T">Item type.</typeparam>
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Splits an enumerable into chunks of a specified size.
        /// </summary>
        /// <param name="source">IEnumerable instance.</param>
        /// <param name="size">Chunk size.</param>
        /// <typeparam name="T">Item type.</typeparam>
        public static IEnumerable<T[]> Chunkify<T>(
            this IEnumerable<T> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var chunk = new T[size];
                    chunk[0] = iter.Current;
                    for (var i = 1; i < size && iter.MoveNext(); i++)
                    {
                        chunk[i] = iter.Current;
                    }

                    yield return chunk;
                }
            }
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the <see cref="T:System.Collections.Generic.List`1" />.
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
    }
}
