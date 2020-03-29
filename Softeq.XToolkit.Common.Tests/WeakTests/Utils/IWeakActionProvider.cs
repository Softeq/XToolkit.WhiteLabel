using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface IWeakActionProvider
    {
        WeakAction GetWeakAnonymousAction();
        WeakAction GetWeakPrivateAction();
        WeakAction GetWeakInternalAction();
        WeakAction GetWeakPublicAction();
    }
}
