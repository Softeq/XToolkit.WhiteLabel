using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Executor;

namespace Softeq.XToolkit.Remote
{
    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        public IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder)
        {
            var apiServiceFactory = new RefitApiServiceFactory();
            var httpClientProvider = new ApiServiceProvider<T>(httpClientBuilder, apiServiceFactory);
            var retryStrategy = new PollyExecutorBuilderFactory();

            return new RemoteService<T>(httpClientProvider, retryStrategy);
        }

        public IRemoteService<T> CreateWithAuth<T>(IHttpClientBuilder httpClientBuilder, ISessionContext sessionContext)
        {
            var apiServiceFactory = new RefitApiServiceFactory();
            var httpClientProvider = new ApiServiceProvider<T>(httpClientBuilder, apiServiceFactory);
            var retryStrategy = new PollyExecutorBuilderFactory();

            return new RemoteService<T>(httpClientProvider, retryStrategy, sessionContext);
        }
    }
}
