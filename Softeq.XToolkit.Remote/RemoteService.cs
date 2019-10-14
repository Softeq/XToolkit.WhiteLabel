using System;
using System.Threading.Tasks;
using Polly;
using Polly.Wrap;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteService<T>
    {
        Task Execute(Func<T, Task> operation);
        Task<TResult> Execute<TResult>(Func<T, Task<TResult>> operation);
        Task Execute(Func<T, Task> operation, RequestOptions options);
        Task<TResult> Execute<TResult>(Func<T, Task<TResult>> operation, RequestOptions options);
    }

    public class RemoteService<T> : IRemoteService<T>
    {
        private readonly IApiService<T> _apiService;

        public RemoteService(
            IApiService<T> refitService)
        {
            _apiService = refitService;
        }

        public Task Execute(Func<T, Task> operation)
        {
            return Execute(operation, GetDefaultOptions());
        }

        public Task<TResult> Execute<TResult>(Func<T, Task<TResult>> operation)
        {
            return Execute(operation, GetDefaultOptions());
        }

        public Task Execute(Func<T, Task> operation, RequestOptions options)
        {
            return Execute(operation, options.Priority, options.RetryCount, options.ShouldRetry, options.Timeout);
        }

        public Task<TResult> Execute<TResult>(Func<T, Task<TResult>> operation, RequestOptions options)
        {
            return Execute<TResult>(operation, options.Priority, options.RetryCount, options.ShouldRetry, options.Timeout);
        }

        public async Task Execute(
            Func<T, Task> operation,
            Priority priority,
            int retryCount,
            Func<Exception, bool> shouldRetry,
            int timeout)
        {
            var service = _apiService.GetByPriority(priority);

            var policy = GetWrappedPolicy(retryCount, shouldRetry, timeout);

            await policy.ExecuteAsync(() => operation.Invoke(service));
        }

        public async Task<TResult> Execute<TResult>(
            Func<T, Task<TResult>> operation,
            Priority priority,
            int retryCount,
            Func<Exception, bool> shouldRetry,
            int timeout)
        {
            var service = _apiService.GetByPriority(priority);

            var policy = GetWrappedPolicy<TResult>(retryCount, shouldRetry, timeout);

            return await policy.ExecuteAsync(() => operation.Invoke(service));
        }

        protected virtual AsyncPolicyWrap GetWrappedPolicy(int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .RetryAsync(retryCount);
            var timeoutPolicy = Policy.TimeoutAsync(timeout);

            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        }

        protected virtual AsyncPolicyWrap<TResult> GetWrappedPolicy<TResult>(int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .WaitAndRetryAsync(retryCount, RetryAttempt)
                                    .AsAsyncPolicy<TResult>();
            var timeoutPolicy = Policy.TimeoutAsync<TResult>(timeout);

            return Policy.WrapAsync<TResult>(retryPolicy, timeoutPolicy);
        }

        protected virtual TimeSpan RetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));

        private RequestOptions GetDefaultOptions()
        {
            return RequestOptions.DefaultRequestOptions
                ?? new RequestOptions
                {
                    Priority = RequestOptions.DefaultPriority,
                    RetryCount = RequestOptions.DefaultRetryCount,
                    Timeout = RequestOptions.DefaultTimeout,
                    ShouldRetry = RequestOptions.DefaultShouldRetry
                };
        }
    }
}
