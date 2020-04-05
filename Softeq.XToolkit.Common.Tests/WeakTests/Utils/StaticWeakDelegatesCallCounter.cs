// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    internal static class StaticWeakDelegatesCallCounter
    {
        private static ICallCounter _callCounter;

        public static IDisposable WithCallCounter(ICallCounter callCounter)
        {
            _callCounter = callCounter;

            return new DisposableCleanupAction(() => _callCounter = null);
        }

        public static WeakAction GetWeakAnonymousActionWithStaticReference() => new WeakAction(() => _callCounter.OnActionCalled());
        public static WeakAction GetWeakStaticAction() => new WeakAction(StaticAction);

        public static WeakAction<T> GetWeakAnonymousActionWithStaticReference<T>() => new WeakAction<T>(x => _callCounter.OnActionCalled(x));
        public static WeakAction<T> GetWeakStaticAction<T>() => new WeakAction<T>(StaticAction);

        public static WeakFunc<TOut> GetWeakAnonymousFuncWithStaticReference<TOut>() => new WeakFunc<TOut>(() => _callCounter.OnFuncCalled<TOut>());
        public static WeakFunc<TOut> GetWeakStaticFunc<TOut>() => new WeakFunc<TOut>(StaticFunc<TOut>);

        public static WeakFunc<TIn, TOut> GetWeakAnonymousFuncWithStaticReference<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => _callCounter.OnFuncCalled<TIn, TOut>(x));
        public static WeakFunc<TIn, TOut> GetWeakStaticFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(StaticFunc<TIn, TOut>);

        private static void StaticAction() => _callCounter.OnActionCalled();
        private static void StaticAction<TIn>(TIn parameter) => _callCounter.OnActionCalled(parameter);
        private static TOut StaticFunc<TOut>() => _callCounter.OnFuncCalled<TOut>();
        private static TOut StaticFunc<TIn, TOut>(TIn parameter) => _callCounter.OnFuncCalled<TIn, TOut>(parameter);
    }
}
