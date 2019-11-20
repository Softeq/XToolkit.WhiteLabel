using System;
using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Timeout;
using Softeq.XToolkit.Remote.Exceptions;

namespace Softeq.XToolkit.Remote.Executor
{
    public class PollyExecutorBuilder<T> : IExecutorBuilder<T>
    {
        private readonly Type[] _allowedExceptions = {
            typeof(InvalidOperationException),
            typeof(ExpiredRefreshTokenException)
        };

        private readonly IList<IAsyncPolicy<T>> _policies = new List<IAsyncPolicy<T>>();

        public IExecutorBuilder<T> WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            var policy = Policy
                .Handle<Exception>(e =>
                {
                    if (_allowedExceptions.Contains(e.GetType()))
                    {
                        return false;
                    }
                    return shouldRetry?.Invoke(e) ?? true;
                })
                .WaitAndRetryAsync(retryCount, RetryAttempt)
                .AsAsyncPolicy<T>();

            _policies.Add(policy);

            return this;
        }

        public IExecutorBuilder<T> WithTimeout(int timeout)
        {
            var policy = Policy
                .TimeoutAsync(timeout, TimeoutStrategy.Pessimistic)
                .AsAsyncPolicy<T>();

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


    public class PollyExecutorBuilder : IExecutorBuilder
    {
        private readonly Type[] _allowedExceptions = {
            typeof(InvalidOperationException),
            typeof(ExpiredRefreshTokenException)
        };

        private readonly IList<IAsyncPolicy> _policies = new List<IAsyncPolicy>();

        public IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            var policy = Policy
                .Handle<Exception>(e =>
                {
                    if (_allowedExceptions.Contains(e.GetType()))
                    {
                        return false;
                    }
                    return shouldRetry?.Invoke(e) ?? true;
                })
                .WaitAndRetryAsync(retryCount, RetryAttempt);

            _policies.Add(policy);

            return this;
        }

        public IExecutorBuilder WithTimeout(int timeout)
        {
            var policy = Policy
                .TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);

            _policies.Add(policy);

            return this;
        }

        public IAsyncPolicy Build()
        {
            return Policy.WrapAsync(_policies.ToArray());
        }

        protected virtual TimeSpan RetryAttempt(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
