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
    }
}
