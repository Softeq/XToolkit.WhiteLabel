using System;
using System.Collections.Generic;
using Polly;
using Polly.Timeout;

namespace Softeq.XToolkit.Remote.Executor
{
    public class PollyExecutorBuilder<T> : IExecutorBuilder<T>
    {
        private readonly List<IAsyncPolicy<T>> _policies = new List<IAsyncPolicy<T>>();

        public IExecutorBuilder<T> WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            var policy = Policy
                .Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                .WaitAndRetryAsync(retryCount, RetryAttempt)
                .AsAsyncPolicy<T>();

            _policies.Add(policy);

            return this;
        }

        public IExecutorBuilder<T> WithTimeout(int timeout)
        {
            var policy = Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic).AsAsyncPolicy<T>();

            _policies.Add(policy);

            return this;
        }

        public IAsyncPolicy<T> Build()
        {
            return Policy.WrapAsync(_policies.ToArray());
        }

        protected virtual TimeSpan RetryAttempt(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
