// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

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

        private static (IDisposable, TWeakDelegate) CreateWeakDelegateWithCustomTarget<TWeakDelegate>(
            Func<object, TWeakDelegate> weakDelegateFactory)
        {
            var customTarget = new object();
            return (new DisposableReference(customTarget), weakDelegateFactory.Invoke(customTarget));
        }

        private static (IDisposable, IDisposable, TWeakDelegate) CreateWeakDelegateWithCustomTarget<TInstance, TWeakDelegate>(
            Func<TInstance> instanceFactory,
            Func<TInstance, object, TWeakDelegate> weakDelegateFactory)
            where TInstance : class
        {
            var originalTarget = instanceFactory.Invoke();
            var customTarget = new object();

            return (new DisposableReference(customTarget),
                new DisposableReference(originalTarget),
                weakDelegateFactory.Invoke(originalTarget, customTarget));
        }

        private sealed class DisposableReference : IDisposable
        {
            private object? _instance;

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
