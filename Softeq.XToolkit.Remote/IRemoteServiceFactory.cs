using System.Net.Http;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(string baseUrl);

        IRemoteService<T> Create<T>(HttpClient httpClient);
    }
}
