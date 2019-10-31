using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder);
        IRemoteService<T> CreateWithAuth<T>(IHttpClientBuilder httpClientBuilder, ISessionContext sessionContext);
    }
}