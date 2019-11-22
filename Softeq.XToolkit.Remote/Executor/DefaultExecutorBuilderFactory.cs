namespace Softeq.XToolkit.Remote.Executor
{
    public class DefaultExecutorBuilderFactory : IExecutorBuilderFactory
    {
        public IExecutorBuilder Create()
        {
            return new DefaultExecutorBuilder();
        }
    }
}
