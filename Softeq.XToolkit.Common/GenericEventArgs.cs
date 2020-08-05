// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common
{
    public static class GenericEventArgs
    {
        /// <summary>
        ///     Creates new instance of <see cref="GenericEventArgs{T}"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <returns>New instance of <see cref="GenericEventArgs{T}"/>.</returns>
        public static GenericEventArgs<T> From<T>(T value)
        {
            return new GenericEventArgs<T>(value);
        }
    }

    /// <summary>
    ///     Represents the generic <see cref="EventArgs"/> class that contains event data,
    ///     and provides a value to use for events that do not include event data.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public GenericEventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets a value to use for events that do not include event data.
        /// </summary>
        public T Value { get; }
    }
}
