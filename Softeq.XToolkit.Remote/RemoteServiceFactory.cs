﻿using System.Net.Http;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Executor;

namespace Softeq.XToolkit.Remote
{
    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiServiceFactory _apiServiceFactory;
        private readonly IExecutorBuilderFactory _executorBuilderFactory;

        public RemoteServiceFactory()
        {
            _httpClientFactory = new DefaultHttpClientFactory();
            _apiServiceFactory = new RefitApiServiceFactory();
            _executorBuilderFactory = new PollyExecutorBuilderFactory();
        }

        public IRemoteService<T> Create<T>(string baseUrl)
        {
            var httpClient = _httpClientFactory.CreateHttpClient(baseUrl);

            return Create<T>(httpClient);
        }

        public IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder)
        {
            var httpClient = httpClientBuilder.Build();

            return Create<T>(httpClient);
        }

        public IRemoteService<T> Create<T>(HttpClient httpClient)
        {
            var apiService = _apiServiceFactory.CreateService<T>(httpClient);

            return new RemoteService<T>(apiService, _executorBuilderFactory);
        }
    }
}
