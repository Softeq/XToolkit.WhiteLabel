using System;
using Polly;
using Polly.Timeout;

namespace Softeq.XToolkit.Remote
{
    public interface IExecutorFactory
    {
        IAsyncPolicy Create(int retryCount, Func<Exception, bool> shouldRetry, int timeout);
        IAsyncPolicy<TResult> Create<TResult>(int retryCount, Func<Exception, bool> shouldRetry, int timeout);
    }

    public class ExecutorFactory : IExecutorFactory
    {
        public IAsyncPolicy Create(
            int retryCount,
            Func<Exception, bool> shouldRetry,
            int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .RetryAsync(retryCount);
            var timeoutPolicy = Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);

            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        }

        public IAsyncPolicy<TResult> Create<TResult>(
            int retryCount,
            Func<Exception, bool> shouldRetry,
            int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .WaitAndRetryAsync(retryCount, RetryAttempt)
                                    .AsAsyncPolicy<TResult>();
            var timeoutPolicy = Policy.TimeoutAsync<TResult>(timeout, TimeoutStrategy.Pessimistic);

            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        }

        protected virtual TimeSpan RetryAttempt(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
