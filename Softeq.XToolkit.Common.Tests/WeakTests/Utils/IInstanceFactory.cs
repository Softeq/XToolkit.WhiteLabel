namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface IInstanceFactory<out T>
    {
        T Create();
    }
}
