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
    /// <summary>
    ///     The main class to make configurable HTTP requests.
    /// </summary>
    /// <typeparam name="TApiService">Type of API service.</typeparam>
    public class RemoteService<TApiService> : IRemoteService<TApiService>
    {
        private readonly TApiService _apiService;
        private readonly IExecutorBuilderFactory _executorFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteService{TApiService}"/> class.
        /// </summary>
        /// <param name="apiService">Instance of API service.</param>
        /// <param name="executorFactory">The factory instance to create request executor.</param>
        public RemoteService(
            TApiService apiService,
            IExecutorBuilderFactory executorFactory)
        {
            _apiService = apiService;
            _executorFactory = executorFactory;
        }

        /// <inheritdoc />
        public virtual async Task MakeRequest(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions? options = null)
        {
            options ??= RequestOptions.GetDefaultOptions();

            await CreatePolicy(options)
                .ExecuteAsync(ct => operation(_apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
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
