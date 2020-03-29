using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface IGenericWeakActionProvider
    {
        WeakAction<T> GetWeakAnonymousAction<T>();
        WeakAction<T> GetWeakPrivateAction<T>();
        WeakAction<T> GetWeakInternalAction<T>();
        WeakAction<T> GetWeakPublicAction<T>();
    }
}
