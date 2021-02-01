// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Executor;

namespace Softeq.XToolkit.Remote
{
    /// <summary>
    ///     Default factory to create <see cref="IRemoteService{T}"/> instances
    ///     with custom configuration.
    /// </summary>
    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        private readonly IApiServiceFactory _apiServiceFactory;
        private readonly IExecutorBuilderFactory _executorBuilderFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteServiceFactory"/> class.
        /// </summary>
        /// <param name="apiServiceFactory">
        ///     Instance of <see cref="IApiServiceFactory"/>.
        ///     Default value is <see cref="RefitApiServiceFactory"/>.
        /// </param>
        public RemoteServiceFactory(IApiServiceFactory? apiServiceFactory = null)
        {
            _apiServiceFactory = apiServiceFactory ?? new RefitApiServiceFactory();
            _executorBuilderFactory = new DefaultExecutorBuilderFactory();
        }

        /// <inheritdoc />
        public IRemoteService<T> Create<T>(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl), "Parameter can't be null or empty.");
            }

            var httpClient = new HttpClientBuilder().WithBaseUrl(baseUrl).Build();

            return Create<T>(httpClient);
        }

        /// <inheritdoc />
        public IRemoteService<T> Create<T>(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            var apiService = _apiServiceFactory.CreateService<T>(httpClient);

            return new RemoteService<T>(apiService, _executorBuilderFactory);
        }
    }
}
