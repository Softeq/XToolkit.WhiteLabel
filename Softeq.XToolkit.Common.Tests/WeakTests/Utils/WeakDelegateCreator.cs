// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public static class WeakDelegateCreator
    {
        public static (IDisposable Target, TWeakDelegate WeakDelegate) CreateWeakDelegate<TInstance, TWeakDelegate>(
            Func<TInstance> instanceFactory,
            Func<TInstance, TWeakDelegate> weakDelegateFactory)
            where TInstance : class
        {
            var instance = instanceFactory.Invoke();
            return (new DisposableReference(instance), weakDelegateFactory.Invoke(instance));
        }

        public static (IDisposable Target, TWeakDelegate WeakDelegate) CreateWeakDelegateWithCustomTarget<TWeakDelegate>(
            Func<object, TWeakDelegate> weakDelegateFactory)
        {
            var customTarget = new object();
            return (new DisposableReference(customTarget), weakDelegateFactory.Invoke(customTarget));
        }

        public static (IDisposable CustomTarget, IDisposable OriginalTarget, TWeakDelegate WeakDelegate) CreateWeakDelegateWithCustomTarget<TInstance, TWeakDelegate>(
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

        [SuppressMessage(
            "ReSharper",
            "NotAccessedField.Local",
            Justification = "Field is used to store hard reference to an object")]
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
