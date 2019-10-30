using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteService<out TApiService>
    {
        Task Execute(Func<TApiService, CancellationToken, Task> operation, RequestOptions options = null);

        Task<TResult> Execute<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null);
    }

    public class RemoteService<TApiService> : IRemoteService<TApiService>
    {
        private readonly IApiServiceProvider<TApiService> _apiServiceProvider;
        private readonly IExecutorFactory _executorFactory;

        public RemoteService(
            IApiServiceProvider<TApiService> apiServiceProvider,
            IExecutorFactory executorFactory)
        {
            _apiServiceProvider = apiServiceProvider;
            _executorFactory = executorFactory;
        }

        public async Task Execute(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var apiService = _apiServiceProvider.GetByPriority(options.Priority);

            var executor = _executorFactory.Create(options.RetryCount, options.ShouldRetry, options.Timeout);

            await executor
                .ExecuteAsync(ct => operation(apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResult> Execute<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var apiService = _apiServiceProvider.GetByPriority(options.Priority);

            var executor = _executorFactory.Create<TResult>(options.RetryCount, options.ShouldRetry, options.Timeout);

            return await executor
                .ExecuteAsync(ct => operation(apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }
    }
}
