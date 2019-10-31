namespace Softeq.XToolkit.Remote.Executor
{
    public class PollyExecutorBuilderFactory : IExecutorBuilderFactory
    {
//        public IAsyncPolicy Create(
//            int retryCount,
//            Func<Exception, bool> shouldRetry,
//            int timeout)
//        {
//            var retryPolicy = Policy
//                .Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
//                .WaitAndRetryAsync(retryCount, RetryAttempt);
//
//            var timeoutPolicy = Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);
//
//            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
//        }

        public IExecutorBuilder<T> Create<T>()
        {
            return new PollyExecutorBuilder<T>();
        }
    }
}
