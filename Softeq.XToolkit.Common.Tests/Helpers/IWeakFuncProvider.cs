namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public interface IWeakFuncProvider
    {
        WeakFunc<TOut> GetWeakAnonymousFunc<TOut>();
        WeakFunc<TOut> GetWeakPrivateFunc<TOut>();
        WeakFunc<TOut> GetWeakInternalFunc<TOut>();
        WeakFunc<TOut> GetWeakPublicFunc<TOut>();
    }
}
