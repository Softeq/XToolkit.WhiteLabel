namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface IMethodRunner : IActionRunner, IParametrizedActionRunner, IFunctionRunner, IParametrizedFunctionRunner
    {
    }

    public interface IActionRunner
    {
        void RunAnonymousAction();
        void RunPublicAction();
        void RunInternalAction();
        void RunPrivateAction();
    }

    public interface IParametrizedActionRunner
    {
        void RunAnonymousAction<T>(T parameter);
        void RunPublicAction<T>(T parameter);
        void RunInternalAction<T>(T parameter);
        void RunPrivateAction<T>(T parameter);
    }

    public interface IFunctionRunner
    {
        TOut RunAnonymousFunc<TOut>();
        TOut RunPublicFunc<TOut>();
        TOut RunInternalFunc<TOut>();
        TOut RunPrivateFunc<TOut>();
    }

    public interface IParametrizedFunctionRunner
    {
        TOut RunAnonymousFunc<TIn, TOut>(TIn parameter);
        TOut RunPublicFunc<TIn, TOut>(TIn parameter);
        TOut RunInternalFunc<TIn, TOut>(TIn parameter);
        TOut RunPrivateFunc<TIn, TOut>(TIn parameter);
    }
}
