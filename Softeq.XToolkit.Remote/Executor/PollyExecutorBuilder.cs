using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;
using Refit;
using Softeq.XToolkit.Remote.Exceptions;

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

        public IExecutorBuilder<T> WithRefreshToken(Func<Task> refreshToken)
        {
            const int accessTokenExpired = 1;
            const int refreshTokenExpired = 2;

            if (refreshToken == null)
            {
                return this;
            }

            var policy = Policy
                .Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(2, async (exception, retryCount) =>
                {
                    if (retryCount == accessTokenExpired)
                    {
                        await refreshToken().ConfigureAwait(false);
                    }

                    if (retryCount == refreshTokenExpired) // refresh token expired
                    {
                        throw new ExpiredRefreshTokenException();
                    }
                })
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
}
