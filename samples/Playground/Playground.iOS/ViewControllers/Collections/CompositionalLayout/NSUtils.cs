// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Playground.iOS.ViewControllers.Collections.CompositionalLayout
{
    internal static class NSUtils
    {
        // YP: Wrap value to use as NSObject
        // Can be small performance degradation, only for demo.
        // Not recommended for production.
        internal class NS<T> : NSObject
        {
            public NS(T value)
            {
                Value = value;
            }

            public T Value { get; }
        }
    }
}
