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
            const int AccessTokenExpired = 1;
            const int RefreshTokenExpired = 2;

            if (refreshToken == null)
            {
                return this;
            }

            var policy = Policy
                .Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(2, async (exception, attempt) =>
                {
                    switch (attempt)
                    {
                        case AccessTokenExpired:
                            await refreshToken().ConfigureAwait(false);
                            break;
                        case RefreshTokenExpired:
                            throw new ExpiredRefreshTokenException(exception);
                        default:
                            throw new InvalidOperationException($"Can't handle attempt number: {attempt.ToString()}", exception);
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
