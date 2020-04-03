// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Softeq.XToolkit.Remote.Executor;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public class RemoteService<TApiService> : IRemoteService<TApiService>
    {
        private readonly TApiService _apiService;
        private readonly IExecutorBuilderFactory _executorFactory;

        public RemoteService(
            TApiService apiService,
            IExecutorBuilderFactory executorFactory)
        {
            _apiService = apiService;
            _executorFactory = executorFactory;
        }

        public virtual async Task MakeRequest(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions? options = null)
        {
            options ??= RequestOptions.GetDefaultOptions();

            await CreatePolicy(options)
                .ExecuteAsync(ct => operation(_apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }

        public virtual async Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions? options = null)
        {
            options ??= RequestOptions.GetDefaultOptions();

            return await CreatePolicy(options)
                .AsAsyncPolicy<TResult>()
                .ExecuteAsync(ct => operation(_apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }

        protected virtual IAsyncPolicy CreatePolicy(RequestOptions options)
        {
            return _executorFactory
                .Create()
                .WithRetry(options.RetryCount, options.ShouldRetry)
                .WithTimeout(options.Timeout)
                .Build();
        }
    }
}
