// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Executor;

namespace Softeq.XToolkit.Remote
{
    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        private readonly IApiServiceFactory _apiServiceFactory;
        private readonly IExecutorBuilderFactory _executorBuilderFactory;

        public RemoteServiceFactory()
        {
            _apiServiceFactory = new RefitApiServiceFactory();
            _executorBuilderFactory = new DefaultExecutorBuilderFactory();
        }

        public IRemoteService<T> Create<T>(string baseUrl)
        {
            var httpClient = new HttpClientBuilder(baseUrl).Build();

            return Create<T>(httpClient);
        }

        public IRemoteService<T> Create<T>(HttpClient httpClient)
        {
            var apiService = _apiServiceFactory.CreateService<T>(httpClient);

            return new RemoteService<T>(apiService, _executorBuilderFactory);
        }
    }
}
