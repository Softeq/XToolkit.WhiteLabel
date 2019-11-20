using System;
using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Softeq.XToolkit.Remote.Exceptions;

namespace Softeq.XToolkit.Remote.Executor
{
    public class PollyExecutorBuilder<T> : PollyExecutorBuilderBase, IExecutorBuilder<T>
    {
        protected readonly IList<IAsyncPolicy<T>> Policies = new List<IAsyncPolicy<T>>();

        public IExecutorBuilder<T> WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            var policy = CreateRetry(retryCount, shouldRetry).AsAsyncPolicy<T>();

            Policies.Add(policy);

            return this;
        }

        public IExecutorBuilder<T> WithTimeout(int timeout)
        {
            var policy = CreateTimeout(timeout).AsAsyncPolicy<T>();

            Policies.Add(policy);

            return this;
        }

        public IAsyncPolicy<T> Build()
        {
            return Policy.WrapAsync(Policies.ToArray());
        }
    }

    public abstract class PollyExecutorBuilderBase
    {
        protected readonly Type[] AllowedExceptions = {
            typeof(InvalidOperationException),
            typeof(ExpiredRefreshTokenException)
        };

        protected virtual AsyncRetryPolicy CreateRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            return Policy
                .Handle<Exception>(e =>
                {
                    if (AllowedExceptions.Contains(e.GetType()))
                    {
                        return false;
                    }
                    return shouldRetry?.Invoke(e) ?? true;
                })
                .WaitAndRetryAsync(retryCount, RetryAttempt);
        }

        protected virtual TimeSpan RetryAttempt(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        protected virtual AsyncTimeoutPolicy CreateTimeout(int timeout)
        {
            return Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);
        }
    }
}
