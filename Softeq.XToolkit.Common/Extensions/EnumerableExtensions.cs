// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Returns empty <see cref="T:System.Collections.Generic.IEnumerable`1"/> if source is <see langword="null"/>.
        /// </summary>
        /// <returns>
        ///     Empty <see cref="T:System.Collections.Generic.IEnumerable`1"/> if source is <see langword="null"/>
        ///     otherwise return source.
        /// </returns>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">Item type.</typeparam>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Apply specified action to each item of the current <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
        /// </summary>
        /// <param name="enumerable"><see cref="T:System.Collections.Generic.IEnumerable`1"/> instance.</param>
        /// <param name="action">Action to apply.</param>
        /// <typeparam name="T">Item type.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="enumerable"/> and <paramref name="action"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Splits the current <see cref="T:System.Collections.Generic.IEnumerable`1"/> into chunks of a specified size.
        /// </summary>
        /// <param name="source"><see cref="T:System.Collections.Generic.IEnumerable`1"/> instance.</param>
        /// <param name="size">Chunk size.</param>
        /// <typeparam name="T">Item type.</typeparam>
        /// <returns>
        ///     Collection of arrays. Each array is a chunk of the source collection and will have the specified size,
        ///     the last chunk might have size less than specified.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="size"/> must be greater than 0.
        /// </exception>
        public static IEnumerable<T[]> Chunkify<T>(this IEnumerable<T> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            var chunkIndex = 0;
            var lastChunkIndex = Math.Ceiling(source.Count() / (double) size) - 1;
            var incompleteChunkSize = source.Count() % size;
            var lastChunkSize = incompleteChunkSize == 0 ? size : incompleteChunkSize;

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var chunk = new T[chunkIndex == lastChunkIndex ? lastChunkSize : size];
                    chunk[0] = iter.Current;
                    for (var i = 1; i < size && iter.MoveNext(); i++)
                    {
                        chunk[i] = iter.Current;
                    }

                    chunkIndex++;
                    yield return chunk;
                }
            }
        }
    }
}
