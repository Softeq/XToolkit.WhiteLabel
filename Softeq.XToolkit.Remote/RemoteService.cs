using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteService<out TApiService>
    {
        Task Execute(Func<TApiService, CancellationToken, Task> operation, RequestOptions options = null);
        Task<TResult> Execute<TResult>(Func<TApiService, CancellationToken, Task<TResult>> operation, RequestOptions options = null);
    }

    public class RemoteService<TApiService> : IRemoteService<TApiService>
    {
        private readonly IApiServiceProvider<TApiService> _apiServiceProvider;
        private readonly IExecutorFactory _executorFactory;

        public RemoteService(
            IApiServiceProvider<TApiService> httpClientProvider,
            IExecutorFactory executorFactory)
        {
            _apiServiceProvider = httpClientProvider;
            _executorFactory = executorFactory;
        }

        public async Task Execute(Func<TApiService, CancellationToken, Task> operation, RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var service = _apiServiceProvider.GetByPriority(options.Priority);

            var executor = _executorFactory.Create(options.RetryCount, options.ShouldRetry, options.Timeout);

            await executor.ExecuteAsync(ct => operation(service, ct), options.CancellationToken);
        }

        public async Task<TResult> Execute<TResult>(Func<TApiService, CancellationToken, Task<TResult>> operation, RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var service = _apiServiceProvider.GetByPriority(options.Priority);

            var executor = _executorFactory.Create<TResult>(options.RetryCount, options.ShouldRetry, options.Timeout);

            return await executor.ExecuteAsync(ct => operation(service, ct), options.CancellationToken);
        }
    }
}
