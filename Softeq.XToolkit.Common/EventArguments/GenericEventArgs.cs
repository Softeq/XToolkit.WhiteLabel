// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.EventArguments
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T t)
        {
            Value = t;
        }

        public T Value { get; }
    }

    public static class GenericEventArgs
    {
        public static GenericEventArgs<T> From<T>(T item)
        {
            return new GenericEventArgs<T>(item);
        }
    }
}
