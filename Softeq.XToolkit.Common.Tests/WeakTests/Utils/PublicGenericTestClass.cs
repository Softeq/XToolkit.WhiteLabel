// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public class PublicTestClass<T>
    {
        private readonly IMethodRunner _callCounter;

        public PublicTestClass(IMethodRunner callCounter)
        {
            _callCounter = callCounter;
        }

        #region Instance methods
        private void PrivateInstanceAction() => _callCounter.RunPrivateAction();

        internal void InternalInstanceAction() => _callCounter.RunInternalAction();

        public void PublicInstanceAction() => _callCounter.RunPublicAction();

        private void PrivateInstanceAction(T parameter) => _callCounter.RunPrivateAction(parameter);

        internal void InternalInstanceAction(T parameter) => _callCounter.RunInternalAction(parameter);

        public void PublicInstanceAction(T parameter) => _callCounter.RunPublicAction(parameter);

        private T PrivateInstanceFunc() => _callCounter.RunPrivateFunc<T>();

        internal T InternalInstanceFunc() => _callCounter.RunInternalFunc<T>();

        public T PublicInstanceFunc() => _callCounter.RunPublicFunc<T>();

        private TOut PrivateInstanceFunc<TOut>(T parameter) => _callCounter.RunPrivateFunc<T, TOut>(parameter);

        internal TOut InternalInstanceFunc<TOut>(T parameter) => _callCounter.RunInternalFunc<T, TOut>(parameter);

        public TOut PublicInstanceFunc<TOut>(T parameter) => _callCounter.RunPublicFunc<T, TOut>(parameter);
        #endregion

        #region WeakDelegate providers
#pragma warning disable RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakAnonymousAction() => new WeakAction(() => _callCounter.RunAnanymousAction());
#pragma warning restore RECS0002 // Convert anonymous method to method group
        public WeakAction GetWeakPrivateAction() => new WeakAction(PrivateInstanceAction);
        public WeakAction GetWeakInternalAction() => new WeakAction(InternalInstanceAction);
        public WeakAction GetWeakPublicAction() => new WeakAction(PublicInstanceAction);

#pragma warning disable RECS0002 // Convert anonymous method to method group
        public WeakAction<T> GetWeakAnonymousGenericAction() => new WeakAction<T>(x => _callCounter.RunAnanimousAction(x));
#pragma warning restore RECS0002 // Convert anonymous method to method group
        public WeakAction<T> GetWeakPrivateGenericAction() => new WeakAction<T>(PrivateInstanceAction);
        public WeakAction<T> GetWeakInternalGenericAction() => new WeakAction<T>(InternalInstanceAction);
        public WeakAction<T> GetWeakPublicGenericAction() => new WeakAction<T>(PublicInstanceAction);

        public WeakFunc<T> GetWeakAnonymousFunc() => new WeakFunc<T>(() => _callCounter.RunAnanimousFunc<T>());
        public WeakFunc<T> GetWeakPrivateFunc() => new WeakFunc<T>(PrivateInstanceFunc);
        public WeakFunc<T> GetWeakInternalFunc() => new WeakFunc<T>(InternalInstanceFunc);
        public WeakFunc<T> GetWeakPublicFunc() => new WeakFunc<T>(PublicInstanceFunc);

#pragma warning disable RECS0002 // Convert anonymous method to method group
        public WeakFunc<T, TOut> GetWeakAnonymousFunc<TOut>() => new WeakFunc<T, TOut>(x => _callCounter.RunAnanimousFunc<T, TOut>(x));
#pragma warning restore RECS0002 // Convert anonymous method to method group
        public WeakFunc<T, TOut> GetWeakPrivateFunc<TOut>() => new WeakFunc<T, TOut>(PrivateInstanceFunc<TOut>);
        public WeakFunc<T, TOut> GetWeakInternalFunc<TOut>() => new WeakFunc<T, TOut>(InternalInstanceFunc<TOut>);
        public WeakFunc<T, TOut> GetWeakPublicFunc<TOut>() => new WeakFunc<T, TOut>(PublicInstanceFunc<TOut>);
        #endregion
    }

    public class PublicGenericWeakDelegateProvider :
        IWeakActionProvider, IGenericWeakActionProvider, IWeakFuncProvider, IGenericWeakFuncProvider
    {
        private readonly IMethodRunner _callCounter;

        public PublicGenericWeakDelegateProvider(IMethodRunner callCounter)
        {
            _callCounter = callCounter;
        }

        public WeakAction GetWeakAnonymousAction() => new PublicTestClass<ITestType>(_callCounter).GetWeakAnonymousAction();
        public WeakAction GetWeakPrivateAction() => new PublicTestClass<ITestType>(_callCounter).GetWeakPrivateAction();
        public WeakAction GetWeakInternalAction() => new PublicTestClass<ITestType>(_callCounter).GetWeakInternalAction();
        public WeakAction GetWeakPublicAction() => new PublicTestClass<ITestType>(_callCounter).GetWeakPublicAction();

        public WeakAction<T> GetWeakAnonymousAction<T>() => new PublicTestClass<T>(_callCounter).GetWeakAnonymousGenericAction();
        public WeakAction<T> GetWeakPrivateAction<T>() => new PublicTestClass<T>(_callCounter).GetWeakPrivateGenericAction();
        public WeakAction<T> GetWeakInternalAction<T>() => new PublicTestClass<T>(_callCounter).GetWeakInternalGenericAction();
        public WeakAction<T> GetWeakPublicAction<T>() => new PublicTestClass<T>(_callCounter).GetWeakPublicGenericAction();

        public WeakFunc<TOut> GetWeakAnonymousFunc<TOut>() => new PublicTestClass<TOut>(_callCounter).GetWeakAnonymousFunc();
        public WeakFunc<TOut> GetWeakPrivateFunc<TOut>() => new PublicTestClass<TOut>(_callCounter).GetWeakPrivateFunc();
        public WeakFunc<TOut> GetWeakInternalFunc<TOut>() => new PublicTestClass<TOut>(_callCounter).GetWeakInternalFunc();
        public WeakFunc<TOut> GetWeakPublicFunc<TOut>() => new PublicTestClass<TOut>(_callCounter).GetWeakPublicFunc();

        public WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>() => new PublicTestClass<TIn>(_callCounter).GetWeakAnonymousFunc<TOut>();
        public WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>() => new PublicTestClass<TIn>(_callCounter).GetWeakPrivateFunc<TOut>();
        public WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>() => new PublicTestClass<TIn>(_callCounter).GetWeakInternalFunc<TOut>();
        public WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>() => new PublicTestClass<TIn>(_callCounter).GetWeakPublicFunc<TOut>();
    }
}
