// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Weak
{
    public static class WeakReferenceEx
    {
        /// <summary>
        ///     Initializes a new instance of the WeakReferenceEx class, referencing the specified object.
        /// </summary>
        /// <returns>New WeakReferenceEx object.</returns>
        /// <param name="target">The object to track.</param>
        /// <typeparam name="T">Target type.</typeparam>
        public static WeakReferenceEx<T> Create<T>(T target) where T : class
        {
            return new WeakReferenceEx<T>(target);
        }
    }

    /// <summary>
    ///     Represents a weak reference, which references an object while still allowing that object to be reclaimed by garbage
    ///     collection.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    public class WeakReferenceEx<T> where T : class
    {
        private readonly WeakReference<T> _weakReference;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakReferenceEx{T}"/> class, referencing the specified object.
        /// </summary>
        /// <param name="obj">The object to track.</param>
        public WeakReferenceEx(T obj)
        {
            _weakReference = new WeakReference<T>(obj);
        }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        /// <value>Return target object if the target was retrieved; otherwise, null.</value>
        public T? Target => _weakReference.TryGetTarget(out var target) ? target : default!;
    }
}
