using System;
using System.Threading;
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

    public class RestRemoteService<T> : IRemoteService<T>
    {
        private readonly IRefitService<T> _refitService;

        public RestRemoteService(
            IRefitService<T> refitService)
        {
            _refitService = refitService;
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

        public async Task Execute(Func<T, Task> operation, Priority priority, int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var service = _refitService.GetByPriority(priority);

            var policy = GetWrappedPolicy(retryCount, shouldRetry, timeout);

            await policy.ExecuteAsync(() => operation.Invoke(service));
        }

        public async Task<TResult> Execute<TResult>(Func<T, Task<TResult>> operation, Priority priority, int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var service = _refitService.GetByPriority(priority);

            var policy = GetWrappedPolicy<TResult>(retryCount, shouldRetry, timeout);

            return await policy.ExecuteAsync(() => operation.Invoke(service));
        }

        private static AsyncPolicyWrap GetWrappedPolicy(int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .RetryAsync(retryCount);
            var timeoutPolicy = Policy.TimeoutAsync(timeout);

            return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        }

        private static AsyncPolicyWrap<TResult> GetWrappedPolicy<TResult>(int retryCount, Func<Exception, bool> shouldRetry, int timeout)
        {
            var retryPolicy = Policy.Handle<Exception>(e => shouldRetry?.Invoke(e) ?? true)
                                    .RetryAsync(retryCount)
                                    .AsAsyncPolicy<TResult>();
            var timeoutPolicy = Policy.TimeoutAsync<TResult>(timeout);

            return Policy.WrapAsync<TResult>(retryPolicy, timeoutPolicy);
        }

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
