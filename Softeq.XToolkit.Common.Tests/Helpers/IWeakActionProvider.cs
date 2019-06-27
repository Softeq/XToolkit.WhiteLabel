namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public interface IWeakActionProvider
    {
        WeakAction GetWeakAnonymousAction();
        WeakAction GetWeakPrivateAction();
        WeakAction GetWeakInternalAction();
        WeakAction GetWeakPublicAction();
    }
}
