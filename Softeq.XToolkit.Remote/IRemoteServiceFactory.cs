using Softeq.XToolkit.Remote.Client;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder);
    }
}
