// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup", Justification = "Lambdas are used on purpose")]
    internal sealed class WeakDelegatesCallCounter
    {
        private readonly ICallCounter _callCounter;

        public WeakDelegatesCallCounter()
        {
        }

        public WeakDelegatesCallCounter(ICallCounter callCounter)
        {
            _callCounter = callCounter;
        }

        public WeakAction GetWeakInstanceAction() => new WeakAction(InstanceAction);
        public WeakAction GetWeakAnonymousActionWithInstanceReference() => new WeakAction(() => _callCounter.OnActionCalled());
        public static WeakAction GetWeakAnonymousActionWithoutReferences() => new WeakAction(() => { });
        public static WeakAction GetWeakAnonymousActionWithLocalReference(ICallCounter callCounter)
        {
            return new WeakAction(() => callCounter.OnActionCalled());
        }

        public WeakAction<T> GetWeakInstanceAction<T>() => new WeakAction<T>(InstanceAction);
        public WeakAction<T> GetWeakAnonymousActionWithInstanceReference<T>() => new WeakAction<T>(x => _callCounter.OnActionCalled(x));
        public static WeakAction<TIn> GetWeakAnonymousActionWithoutReferences<TIn>() => new WeakAction<TIn>(x => { });
        public static WeakAction<TIn> GetWeakAnonymousActionWithLocalReference<TIn>(ICallCounter callCounter)
        {
            return new WeakAction<TIn>(x => callCounter.OnActionCalled(x));
        }

        public WeakFunc<TOut> GetWeakAnonymousFuncWithInstanceReference<TOut>() => new WeakFunc<TOut>(() => _callCounter.OnFuncCalled<TOut>());
        public WeakFunc<TOut> GetWeakInstanceFunc<TOut>() => new WeakFunc<TOut>(InstanceFunc<TOut>);
        public static WeakFunc<TOut> GetWeakAnonymousFuncWithoutReferences<TOut>() => new WeakFunc<TOut>(() => default);
        public static WeakFunc<TOut> GetWeakAnonymousFuncWithLocalReference<TOut>(ICallCounter callCounter)
        {
            return new WeakFunc<TOut>(() => callCounter.OnFuncCalled<TOut>());
        }

        public WeakFunc<TIn, TOut> GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => _callCounter.OnFuncCalled<TIn, TOut>(x));
        public WeakFunc<TIn, TOut> GetWeakInstanceFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(InstanceFunc<TIn, TOut>);
        public static WeakFunc<TIn, TOut> GetWeakAnonymousFuncWithoutReferences<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => default);
        public static WeakFunc<TIn, TOut> GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(ICallCounter callCounter)
        {
            return new WeakFunc<TIn, TOut>(x => callCounter.OnFuncCalled<TIn, TOut>(x));
        }

        private void InstanceAction() => _callCounter.OnActionCalled();
        private void InstanceAction<TIn>(TIn parameter) => _callCounter.OnActionCalled(parameter);
        private TOut InstanceFunc<TOut>() => _callCounter.OnFuncCalled<TOut>();
        private TOut InstanceFunc<TIn, TOut>(TIn parameter) => _callCounter.OnFuncCalled<TIn, TOut>(parameter);
    }
}
