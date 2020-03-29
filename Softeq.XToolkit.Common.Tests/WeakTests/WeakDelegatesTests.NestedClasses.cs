using Softeq.XToolkit.Common.Tests.Helpers;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        public class NestedTestClass :
            IWeakActionProvider, IGenericWeakActionProvider, IWeakFuncProvider, IGenericWeakFuncProvider
        {
            private readonly IMethodRunner _callCounter;

            public NestedTestClass(IMethodRunner callCounter)
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
            public WeakAction GetWeakAnonymousAction() => new WeakAction(() => _callCounter.RunAnonymousAction());
#pragma warning restore RECS0002 // Convert anonymous method to method group
            public WeakAction GetWeakPrivateAction() => new WeakAction(PrivateInstanceAction);
            public WeakAction GetWeakInternalAction() => new WeakAction(InternalInstanceAction);
            public WeakAction GetWeakPublicAction() => new WeakAction(PublicInstanceAction);

            public WeakAction<T> GetWeakAnonymousAction<T>() => new WeakAction<T>(x => _callCounter.RunAnonymousAction(x));
            public WeakAction<T> GetWeakPrivateAction<T>() => new WeakAction<T>(PrivateInstanceAction);
            public WeakAction<T> GetWeakInternalAction<T>() => new WeakAction<T>(InternalInstanceAction);
            public WeakAction<T> GetWeakPublicAction<T>() => new WeakAction<T>(PublicInstanceAction);

            public WeakFunc<TOut> GetWeakAnonymousFunc<TOut>() => new WeakFunc<TOut>(() => _callCounter.RunAnonymousFunc<TOut>());
            public WeakFunc<TOut> GetWeakPrivateFunc<TOut>() => new WeakFunc<TOut>(PrivateInstanceFunc<TOut>);
            public WeakFunc<TOut> GetWeakInternalFunc<TOut>() => new WeakFunc<TOut>(InternalInstanceFunc<TOut>);
            public WeakFunc<TOut> GetWeakPublicFunc<TOut>() => new WeakFunc<TOut>(PublicInstanceFunc<TOut>);

            public WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(x => _callCounter.RunAnonymousFunc<TIn, TOut>(x));
            public WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PrivateInstanceFunc<TIn, TOut>);
            public WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(InternalInstanceFunc<TIn, TOut>);
            public WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>() => new WeakFunc<TIn, TOut>(PublicInstanceFunc<TIn, TOut>);
            #endregion
        }

        public class NestedTestClass<T>
        {
            private readonly IMethodRunner _callCounter;

            public NestedTestClass(IMethodRunner callCounter)
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
            public WeakAction GetWeakAnonymousAction() => new WeakAction(() => _callCounter.RunAnonymousAction());
#pragma warning restore RECS0002 // Convert anonymous method to method group
            public WeakAction GetWeakPrivateAction() => new WeakAction(PrivateInstanceAction);
            public WeakAction GetWeakInternalAction() => new WeakAction(InternalInstanceAction);
            public WeakAction GetWeakPublicAction() => new WeakAction(PublicInstanceAction);

#pragma warning disable RECS0002 // Convert anonymous method to method group
            public WeakAction<T> GetWeakAnonymousGenericAction() => new WeakAction<T>(x => _callCounter.RunAnonymousAction(x));
#pragma warning restore RECS0002 // Convert anonymous method to method group
            public WeakAction<T> GetWeakPrivateGenericAction() => new WeakAction<T>(PrivateInstanceAction);
            public WeakAction<T> GetWeakInternalGenericAction() => new WeakAction<T>(InternalInstanceAction);
            public WeakAction<T> GetWeakPublicGenericAction() => new WeakAction<T>(PublicInstanceAction);

            public WeakFunc<T> GetWeakAnonymousFunc() => new WeakFunc<T>(() => _callCounter.RunAnonymousFunc<T>());
            public WeakFunc<T> GetWeakPrivateFunc() => new WeakFunc<T>(PrivateInstanceFunc);
            public WeakFunc<T> GetWeakInternalFunc() => new WeakFunc<T>(InternalInstanceFunc);
            public WeakFunc<T> GetWeakPublicFunc() => new WeakFunc<T>(PublicInstanceFunc);

#pragma warning disable RECS0002 // Convert anonymous method to method group
            public WeakFunc<T, TOut> GetWeakAnonymousFunc<TOut>() => new WeakFunc<T, TOut>(x => _callCounter.RunAnonymousFunc<T, TOut>(x));
#pragma warning restore RECS0002 // Convert anonymous method to method group
            public WeakFunc<T, TOut> GetWeakPrivateFunc<TOut>() => new WeakFunc<T, TOut>(PrivateInstanceFunc<TOut>);
            public WeakFunc<T, TOut> GetWeakInternalFunc<TOut>() => new WeakFunc<T, TOut>(InternalInstanceFunc<TOut>);
            public WeakFunc<T, TOut> GetWeakPublicFunc<TOut>() => new WeakFunc<T, TOut>(PublicInstanceFunc<TOut>);
            #endregion
        }

        public class NestedGenericWeakDelegateProvider : IWeakActionProvider,
            IGenericWeakActionProvider, IWeakFuncProvider, IGenericWeakFuncProvider
        {
            private readonly IMethodRunner _callCounter;

            public NestedGenericWeakDelegateProvider(IMethodRunner callCounter)
            {
                _callCounter = callCounter;
            }

            public WeakAction GetWeakAnonymousAction() => new NestedTestClass<ITestType>(_callCounter).GetWeakAnonymousAction();
            public WeakAction GetWeakPrivateAction() => new NestedTestClass<ITestType>(_callCounter).GetWeakPrivateAction();
            public WeakAction GetWeakInternalAction() => new NestedTestClass<ITestType>(_callCounter).GetWeakInternalAction();
            public WeakAction GetWeakPublicAction() => new NestedTestClass<ITestType>(_callCounter).GetWeakPublicAction();

            public WeakAction<T> GetWeakAnonymousAction<T>() => new NestedTestClass<T>(_callCounter).GetWeakAnonymousGenericAction();
            public WeakAction<T> GetWeakPrivateAction<T>() => new NestedTestClass<T>(_callCounter).GetWeakPrivateGenericAction();
            public WeakAction<T> GetWeakInternalAction<T>() => new NestedTestClass<T>(_callCounter).GetWeakInternalGenericAction();
            public WeakAction<T> GetWeakPublicAction<T>() => new NestedTestClass<T>(_callCounter).GetWeakPublicGenericAction();

            public WeakFunc<TOut> GetWeakAnonymousFunc<TOut>() => new NestedTestClass<TOut>(_callCounter).GetWeakAnonymousFunc();
            public WeakFunc<TOut> GetWeakPrivateFunc<TOut>() => new NestedTestClass<TOut>(_callCounter).GetWeakPrivateFunc();
            public WeakFunc<TOut> GetWeakInternalFunc<TOut>() => new NestedTestClass<TOut>(_callCounter).GetWeakInternalFunc();
            public WeakFunc<TOut> GetWeakPublicFunc<TOut>() => new NestedTestClass<TOut>(_callCounter).GetWeakPublicFunc();

            public WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>() => new NestedTestClass<TIn>(_callCounter).GetWeakAnonymousFunc<TOut>();
            public WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>() => new NestedTestClass<TIn>(_callCounter).GetWeakPrivateFunc<TOut>();
            public WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>() => new NestedTestClass<TIn>(_callCounter).GetWeakInternalFunc<TOut>();
            public WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>() => new NestedTestClass<TIn>(_callCounter).GetWeakPublicFunc<TOut>();
        }
    }
}
