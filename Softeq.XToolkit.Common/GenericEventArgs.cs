// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T t)
        {
            Value = t;
        }

        public T Value { get; }

        public static GenericEventArgs<T> Create<T>(T item)
        {
            return new GenericEventArgs<T>(item);
        }
    }

    public static class GenericEventArgs
    {
        public static GenericEventArgs<T> From<T>(T item)
        {
            return new GenericEventArgs<T>(item);
        }
    }
}
