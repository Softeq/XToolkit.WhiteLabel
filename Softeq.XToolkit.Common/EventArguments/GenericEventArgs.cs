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
}