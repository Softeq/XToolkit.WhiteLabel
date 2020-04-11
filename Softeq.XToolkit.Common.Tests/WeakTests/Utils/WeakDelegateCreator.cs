// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public static class WeakDelegateCreator
    {
        public static (IDisposable target, TWeakDelegate weakDelegate) CreateWeakDelegate<TInstance, TWeakDelegate>(
            Func<TInstance> instanceFactory,
            Func<TInstance, TWeakDelegate> weakDelegateFactory)
            where TInstance : class
        {
            var instance = instanceFactory.Invoke();
            return (new DisposableReference(instance), weakDelegateFactory.Invoke(instance));
        }

        public static (IDisposable target, TWeakDelegate weakDelegate) CreateWeakDelegateWithCustomTarget<TWeakDelegate>(
            Func<object, TWeakDelegate> weakDelegateFactory)
        {
            var customTarget = new object();
            return (new DisposableReference(customTarget), weakDelegateFactory.Invoke(customTarget));
        }

        public static (IDisposable customTarget, IDisposable originalTarget, TWeakDelegate weakDelegate) CreateWeakDelegateWithCustomTarget<TInstance, TWeakDelegate>(
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
