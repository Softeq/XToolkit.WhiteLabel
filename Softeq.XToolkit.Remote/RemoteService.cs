using System;
using System.Threading;
using System.Threading.Tasks;
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

        public virtual async Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var executor = _executorFactory
                .Create<TResult>()
                .WithRetry(options.RetryCount, options.ShouldRetry)
                .WithTimeout(options.Timeout)
                .Build();

            return await executor
                .ExecuteAsync(ct => operation(_apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task MakeRequest(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
