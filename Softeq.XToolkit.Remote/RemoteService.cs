using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Executor;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public class RemoteService<TApiService> : IRemoteService<TApiService>
    {
        private readonly IApiServiceProvider<TApiService> _apiServiceProvider;
        private readonly IExecutorBuilderFactory _executorFactory;

        public RemoteService(
            IApiServiceProvider<TApiService> apiServiceProvider,
            IExecutorBuilderFactory executorFactory)
        {
            _apiServiceProvider = apiServiceProvider;
            _executorFactory = executorFactory;
        }

        public async Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var apiService = _apiServiceProvider.GetByPriority(options.Priority);

            var executor = _executorFactory
                .Create<TResult>()
                .WithRetry(options.RetryCount, options.ShouldRetry)
                .WithTimeout(options.Timeout)
                .Build();

            return await executor
                .ExecuteAsync(ct => operation(apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }
    }
}
