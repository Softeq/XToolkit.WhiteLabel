// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Softeq.XToolkit.Remote.Exceptions;

namespace Softeq.XToolkit.Remote.Executor
{
    /// <summary>
    ///     The default builder for configuring Polly <see cref="T:Polly.IAsyncPolicy"/> instances.
    /// </summary>
    public class DefaultExecutorBuilder : IExecutorBuilder
    {
        private readonly IList<IAsyncPolicy> _policies = new List<IAsyncPolicy>();

        // Ignore RefreshTokenHttpClientHandler exceptions.
        protected readonly Type[] AllowedExceptions =
        {
            typeof(InvalidOperationException),
            typeof(ExpiredRefreshTokenException)
        };

        /// <inheritdoc />
        public IExecutorBuilder WithTimeout(int timeout)
        {
            _policies.Add(CreateTimeoutPolicy(timeout));
            return this;
        }

        /// <inheritdoc />
        public IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry)
        {
            if (shouldRetry == null)
            {
                throw new ArgumentNullException(nameof(shouldRetry));
            }

            _policies.Add(CreateRetryPolicy(retryCount, shouldRetry));
            return this;
        }

        /// <inheritdoc />
        public IAsyncPolicy Build()
        {
            if (_policies.Count > 0)
            {
                return Policy.WrapAsync(_policies.ToArray());
            }

            return Policy.NoOpAsync();
        }

        protected virtual AsyncTimeoutPolicy CreateTimeoutPolicy(int timeout)
        {
            // https://github.com/App-vNext/Polly/wiki/Timeout#use-optimistic-timeout-with-httpclient-calls
            return Policy.TimeoutAsync(timeout, TimeoutStrategy.Optimistic);
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

            return shouldRetry.Invoke(e);
        }

        protected virtual TimeSpan RetrySleepDurationProvider(int attemptNumber)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
