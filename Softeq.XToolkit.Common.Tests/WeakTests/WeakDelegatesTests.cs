// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Some generic parameters used just for test case generation")]
    public partial class WeakDelegatesTests
    {
        private static (IDisposable, TWeakDelegate) CreateWeakDelegate<TInstance, TWeakDelegate>(
            Func<TInstance> instanceFactory,
            Func<TInstance, TWeakDelegate> weakDelegateFactory)
            where TInstance : class
        {
            var instance = instanceFactory.Invoke();
            return (new DisposableReference(instance), weakDelegateFactory.Invoke(instance));
        }

        private sealed class DisposableReference : IDisposable
        {
            private object _instance;

            public DisposableReference(object instance)
            {
                _instance = instance;
            }

            public void Dispose()
            {
                _instance = null;
            }
        }
    }
}
