// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Softeq.XToolkit.Common.Weak;

#nullable enable

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    internal static class StaticWeakDelegatesCallCounter
    {
        private static ICallCounter? _callCounter;

        public static IDisposable WithCallCounter(ICallCounter callCounter)
        {
            _callCounter = callCounter;

            return new DisposableCleanupAction(() => _callCounter = null);
        }

        public static WeakAction GetWeakStaticAction() => new WeakAction(StaticAction);
        public static WeakAction GetWeakStaticAction(object? target) => new WeakAction(target, StaticAction);
        public static WeakAction GetWeakAnonymousActionWithStaticReference() => new WeakAction(() => _callCounter?.OnActionCalled());

        public static WeakAction<T> GetWeakStaticAction<T>() => new WeakAction<T>(StaticAction);
        public static WeakAction<T> GetWeakStaticAction<T>(object? target) => new WeakAction<T>(target, StaticAction);
        public static WeakAction<T> GetWeakAnonymousActionWithStaticReference<T>() => new WeakAction<T>(x => _callCounter?.OnActionCalled(x));

        public static WeakFunc<TOut> GetWeakStaticFunc<TOut>() => new WeakFunc<TOut>(StaticFunc<TOut>);
        public static WeakFunc<TOut> GetWeakStaticFunc<TOut>(object? target) => new WeakFunc<TOut>(target, StaticFunc<TOut>);
        public static WeakFunc<TOut> GetWeakAnonymousFuncWithStaticReference<TOut>() =>
            new WeakFunc<TOut>(() => _callCounter != null ? _callCounter.OnFuncCalled<TOut>() : default);

        public static WeakFunc<TIn, TOut> GetWeakStaticFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(StaticFunc<TIn, TOut>);
        public static WeakFunc<TIn, TOut> GetWeakStaticFunc<TIn, TOut>(object? target) => new WeakFunc<TIn, TOut>(target, StaticFunc<TIn, TOut>);
        public static WeakFunc<TIn, TOut> GetWeakAnonymousFuncWithStaticReference<TIn, TOut>() =>
            new WeakFunc<TIn, TOut>(x => _callCounter != null ? _callCounter.OnFuncCalled<TIn, TOut>(x) : default);

        private static void StaticAction() => _callCounter?.OnActionCalled();

        private static void StaticAction<TIn>(TIn parameter) => _callCounter?.OnActionCalled(parameter);

        [return: MaybeNull]
        private static TOut StaticFunc<TOut>() => _callCounter != null ? _callCounter.OnFuncCalled<TOut>() : default;

        [return: MaybeNull]
        private static TOut StaticFunc<TIn, TOut>(TIn parameter) =>
            _callCounter != null ? _callCounter.OnFuncCalled<TIn, TOut>(parameter) : default;
    }
}
