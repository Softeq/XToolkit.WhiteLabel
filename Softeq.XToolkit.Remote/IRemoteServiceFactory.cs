using System.Net.Http;
using Softeq.XToolkit.Remote.Client;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(string baseUrl);

        IRemoteService<T> Create<T>(IHttpClientBuilder httpClientBuilder);

        IRemoteService<T> Create<T>(HttpClient httpClient);
    }
}
