// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    internal class InternalStaticTestClass : IWeakActionProvider,
        IGenericWeakActionProvider, IWeakFuncProvider, IGenericWeakFuncProvider
    {
        public static IMethodRunner CallCounter { get; set; }

        #region Static methods
        private static void PrivateStaticAction() => CallCounter.RunPrivateAction();

        internal static void InternalStaticAction() => CallCounter.RunInternalAction();

        public static void PublicStaticAction() => CallCounter.RunPublicAction();

        private static void PrivateStaticAction<T>(T parameter) => CallCounter.RunPrivateAction(parameter);

        internal static void InternalStaticAction<T>(T parameter) => CallCounter.RunInternalAction(parameter);

        public static void PublicStaticAction<T>(T parameter) => CallCounter.RunPublicAction(parameter);

        private static T PrivateStaticFunc<T>() => CallCounter.RunPrivateFunc<T>();

        internal static T InternalStaticFunc<T>() => CallCounter.RunInternalFunc<T>();

        public static T PublicStaticFunc<T>() => CallCounter.RunPublicFunc<T>();

        private static TOut PrivateStaticFunc<TIn, TOut>(TIn parameter) => CallCounter.RunPrivateFunc<TIn, TOut>(parameter);

        internal static TOut InternalStaticFunc<TIn, TOut>(TIn parameter) => CallCounter.RunInternalFunc<TIn, TOut>(parameter);

        public static TOut PublicStaticFunc<TIn, TOut>(TIn parameter) => CallCounter.RunPublicFunc<TIn, TOut>(parameter);
        #endregion

        #region WeakDelegate providers
#pragma warning disable RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakAnonymousAction() => new WeakAction(() => CallCounter.RunAnanymousAction());
#pragma warning restore RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakPrivateAction() => new WeakAction(PrivateStaticAction);
        public WeakAction GetWeakInternalAction() => new WeakAction(InternalStaticAction);
        public WeakAction GetWeakPublicAction() => new WeakAction(PublicStaticAction);

        public WeakAction<T> GetWeakAnonymousAction<T>() => new WeakAction<T>(x => CallCounter.RunAnanimousAction(x));
        public WeakAction<T> GetWeakPrivateAction<T>() => new WeakAction<T>(PrivateStaticAction);
        public WeakAction<T> GetWeakInternalAction<T>() => new WeakAction<T>(InternalStaticAction);
        public WeakAction<T> GetWeakPublicAction<T>() => new WeakAction<T>(PublicStaticAction);

        public WeakFunc<TOut> GetWeakAnonymousFunc<TOut>() => new WeakFunc<TOut>(() => CallCounter.RunAnanimousFunc<TOut>());
        public WeakFunc<TOut> GetWeakPrivateFunc<TOut>() => new WeakFunc<TOut>(PrivateStaticFunc<TOut>);
        public WeakFunc<TOut> GetWeakInternalFunc<TOut>() => new WeakFunc<TOut>(InternalStaticFunc<TOut>);
        public WeakFunc<TOut> GetWeakPublicFunc<TOut>() => new WeakFunc<TOut>(PublicStaticFunc<TOut>);

        public WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => CallCounter.RunAnanimousFunc<TIn, TOut>(x));
        public WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PrivateStaticFunc<TIn, TOut>);
        public WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(InternalStaticFunc<TIn, TOut>);
        public WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PublicStaticFunc<TIn, TOut>);
        #endregion
    }
}
