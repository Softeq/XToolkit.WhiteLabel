namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public interface IGenericWeakFuncProvider
    {
        WeakFunc<TIn, TOut> GetWeakAnonymousFunc<TIn, TOut>();
        WeakFunc<TIn, TOut> GetWeakPrivateFunc<TIn, TOut>();
        WeakFunc<TIn, TOut> GetWeakInternalFunc<TIn, TOut>();
        WeakFunc<TIn, TOut> GetWeakPublicFunc<TIn, TOut>();
    }
}
