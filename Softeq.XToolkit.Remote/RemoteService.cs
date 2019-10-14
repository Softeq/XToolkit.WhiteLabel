using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;
using Polly.Wrap;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteService<T>
    {
        Task Execute(Func<T, CancellationToken, Task> operation);
        Task<TResult> Execute<TResult>(Func<T, CancellationToken, Task<TResult>> operation);
        Task Execute(Func<T, CancellationToken, Task> operation, RequestOptions options);
        Task<TResult> Execute<TResult>(Func<T, CancellationToken, Task<TResult>> operation, RequestOptions options);
    }

    public class RemoteService<T> : IRemoteService<T>
    {
        private readonly IHttpClientProvider<T> _apiService;

        public RemoteService(
            IHttpClientProvider<T> refitService)
        {
            _apiService = refitService;
        }

        public Task Execute(Func<T, CancellationToken, Task> operation)
        {
            return Execute(operation, RequestOptions.GetDefaultOptions());
        }

        public Task<TResult> Execute<TResult>(Func<T, CancellationToken, Task<TResult>> operation)
        {
            return Execute(operation, RequestOptions.GetDefaultOptions());
        }

        public async Task Execute(Func<T, CancellationToken, Task> operation, RequestOptions options)
        {
            var service = _apiService.GetByPriority(options.Priority);

            var policy = GetWrappedPolicy(options.RetryCount, options.ShouldRetry, options.Timeout);

            await policy.ExecuteAsync(ct => operation.Invoke(service, ct), options.CancellationToken);
        }

        public async Task<TResult> Execute<TResult>(Func<T, CancellationToken, Task<TResult>> operation, RequestOptions options)
        {
            var service = _apiService.GetByPriority(options.Priority);

            var policy = GetWrappedPolicy<TResult>(options.RetryCount, options.ShouldRetry, options.Timeout);

            return await policy.ExecuteAsync(ct => operation.Invoke(service, ct), options.CancellationToken);
        }

        protected virtual AsyncPolicyWrap GetWrappedPolicy(
            int retryCount,
            Func<Exception, bool> shouldRetry,
            int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .RetryAsync(retryCount);
            var timeoutPolicy = Policy.TimeoutAsync(timeout, TimeoutStrategy.Pessimistic);

            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        }

        protected virtual AsyncPolicyWrap<TResult> GetWrappedPolicy<TResult>(
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
