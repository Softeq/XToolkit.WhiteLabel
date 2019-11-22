using System;
using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Softeq.XToolkit.Remote.Exceptions;

namespace Softeq.XToolkit.Remote.Executor
{
    public class DefaultExecutorBuilder : IExecutorBuilder
    {
        private readonly IList<IAsyncPolicy> _policies = new List<IAsyncPolicy>();

        protected readonly Type[] AllowedExceptions = {
            typeof(InvalidOperationException),
            typeof(ExpiredRefreshTokenException)
        };

        public IExecutorBuilder WithTimeout(int timeout)
        {
            _policies.Add(CreateTimeoutPolicy(timeout));
            return this;
        }

        public IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            _policies.Add(CreateRetryPolicy(retryCount, shouldRetry));
            return this;
        }

        public IAsyncPolicy Build()
        {
            return Policy.WrapAsync(_policies.ToArray());
        }

        protected virtual AsyncTimeoutPolicy CreateTimeoutPolicy(int timeout)
        {
            return Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);
        }

        protected virtual AsyncRetryPolicy CreateRetryPolicy(int retryCount, Func<Exception, bool> shouldRetry)
        {
            return Policy
                .Handle<Exception>(ex => OnHandleException(ex, shouldRetry))
                .WaitAndRetryAsync(retryCount, RetrySleepDurationProvider);
        }

        protected virtual bool OnHandleException(Exception e, Func<Exception, bool> shouldRetry)
        {
            if (AllowedExceptions.Contains(e.GetType()))
            {
                return false;
            }
            return shouldRetry?.Invoke(e) ?? true;
        }

        protected virtual TimeSpan RetrySleepDurationProvider(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
