namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public interface IMethodRunner
    {
        void RunAnanymousAction();
        void RunAnanimousAction<T>(T parameter);

        void RunPublicAction();
        void RunPublicAction<T>(T parameter);

        void RunInternalAction();
        void RunInternalAction<T>(T parameter);

        void RunPrivateAction();
        void RunPrivateAction<T>(T parameter);

        TOut RunAnanimousFunc<TOut>();
        TOut RunAnanimousFunc<TIn, TOut>(TIn parameter);

        TOut RunPublicFunc<TOut>();
        TOut RunPublicFunc<TIn, TOut>(TIn parameter);

        TOut RunInternalFunc<TOut>();
        TOut RunInternalFunc<TIn, TOut>(TIn parameter);

        TOut RunPrivateFunc<TOut>();
        TOut RunPrivateFunc<TIn, TOut>(TIn parameter);
    }
}
