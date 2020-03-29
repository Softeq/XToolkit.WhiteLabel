using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface IWeakFuncProvider
    {
        WeakFunc<TOut> GetWeakAnonymousFunc<TOut>();
        WeakFunc<TOut> GetWeakPrivateFunc<TOut>();
        WeakFunc<TOut> GetWeakInternalFunc<TOut>();
        WeakFunc<TOut> GetWeakPublicFunc<TOut>();
    }
}
