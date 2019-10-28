using Softeq.XToolkit.Remote.Client;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder);
    }

    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        public IRemoteService<T> Create<T>(string baseUrl)
        {
            var httpClientBuilder = new HttpClientBuilder(baseUrl);

            return Create<T>(httpClientBuilder);
        }

        public IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder)
        {
            var apiServiceFactory = new RefitApiServiceFactory();
            var httpClientFactory = new HttpClientFactory(httpClientBuilder);
            var httpClientProvider = new ApiServiceProvider<T>(httpClientFactory, apiServiceFactory);
            var retryStrategy = new ExecutorFactory();

            return new RemoteService<T>(httpClientProvider, retryStrategy);
        }
    }
}
