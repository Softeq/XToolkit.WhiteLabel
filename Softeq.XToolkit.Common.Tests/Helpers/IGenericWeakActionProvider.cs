namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public interface IGenericWeakActionProvider
    {
        WeakAction<T> GetWeakAnonymousAction<T>();
        WeakAction<T> GetWeakPrivateAction<T>();
        WeakAction<T> GetWeakInternalAction<T>();
        WeakAction<T> GetWeakPublicAction<T>();
    }
}
