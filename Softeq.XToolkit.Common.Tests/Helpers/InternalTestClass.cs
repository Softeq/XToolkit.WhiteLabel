// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Tests.Helpers;

namespace Softeq.XToolkit.Tests.Core.Common.Helpers
{
    internal sealed class InternalTestClass : IWeakActionProvider,
        IGenericWeakActionProvider, IWeakFuncProvider, IGenericWeakFuncProvider
    {
        private readonly IMethodRunner _callCounter;

        public InternalTestClass(IMethodRunner callCounter)
        {
            _callCounter = callCounter;
        }

        #region Instance methods
        private void PrivateInstanceAction() => _callCounter.RunPrivateAction();

        internal void InternalInstanceAction() => _callCounter.RunInternalAction();

        public void PublicInstanceAction() => _callCounter.RunPublicAction();

        private void PrivateInstanceAction<T>(T parameter) => _callCounter.RunPrivateAction(parameter);

        internal void InternalInstanceAction<T>(T parameter) => _callCounter.RunInternalAction(parameter);

        public void PublicInstanceAction<T>(T parameter) => _callCounter.RunPublicAction(parameter);

        private T PrivateInstanceFunc<T>() => _callCounter.RunPrivateFunc<T>();

        internal T InternalInstanceFunc<T>() => _callCounter.RunInternalFunc<T>();

        public T PublicInstanceFunc<T>() => _callCounter.RunPublicFunc<T>();

        private TOut PrivateInstanceFunc<TIn, TOut>(TIn parameter) => _callCounter.RunPrivateFunc<TIn, TOut>(parameter);

        internal TOut InternalInstanceFunc<TIn, TOut>(TIn parameter) => _callCounter.RunInternalFunc<TIn, TOut>(parameter);

        public TOut PublicInstanceFunc<TIn, TOut>(TIn parameter) => _callCounter.RunPublicFunc<TIn, TOut>(parameter);
        #endregion

        #region WeakDelegate providers
#pragma warning disable RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakAnonymousAction() => new WeakAction(() => _callCounter.RunAnanymousAction());
#pragma warning restore RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakPrivateAction() => new WeakAction(PrivateInstanceAction);
        public WeakAction GetWeakInternalAction() => new WeakAction(InternalInstanceAction);
        public WeakAction GetWeakPublicAction() => new WeakAction(PublicInstanceAction);

        public WeakAction<T> GetWeakAnonymousAction<T>() => new WeakAction<T>(x => _callCounter.RunAnanimousAction(x));
        public WeakAction<T> GetWeakPrivateAction<T>() => new WeakAction<T>(PrivateInstanceAction);
        public WeakAction<T> GetWeakInternalAction<T>() => new WeakAction<T>(InternalInstanceAction);
        public WeakAction<T> GetWeakPublicAction<T>() => new WeakAction<T>(PublicInstanceAction);

        public WeakFunc<TOut> GetWeakAnonymousFunc<TOut>() => new WeakFunc<TOut>(() => _callCounter.RunAnanimousFunc<TOut>());
        public WeakFunc<TOut> GetWeakPrivateFunc<TOut>() => new WeakFunc<TOut>(PrivateInstanceFunc<TOut>);
        public WeakFunc<TOut> GetWeakInternalFunc<TOut>() => new WeakFunc<TOut>(InternalInstanceFunc<TOut>);
        public WeakFunc<TOut> GetWeakPublicFunc<TOut>() => new WeakFunc<TOut>(PublicInstanceFunc<TOut>);

        public WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => _callCounter.RunAnanimousFunc<TIn, TOut>(x));
        public WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PrivateInstanceFunc<TIn, TOut>);
        public WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(InternalInstanceFunc<TIn, TOut>);
        public WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PublicInstanceFunc<TIn, TOut>);
        #endregion
    }
}