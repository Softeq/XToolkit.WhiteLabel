namespace Softeq.XToolkit.Remote.Executor
{
    public class PollyExecutorBuilderFactory : IExecutorBuilderFactory
    {
        public IExecutorBuilder<T> Create<T>()
        {
            return new PollyExecutorBuilder<T>();
        }
    }
}
