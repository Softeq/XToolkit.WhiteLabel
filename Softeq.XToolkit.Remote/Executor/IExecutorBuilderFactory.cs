namespace Softeq.XToolkit.Remote.Executor
{
    public interface IExecutorBuilderFactory
    {
        IExecutorBuilder<T> Create<T>();

        IExecutorBuilder Create();
    }
}
