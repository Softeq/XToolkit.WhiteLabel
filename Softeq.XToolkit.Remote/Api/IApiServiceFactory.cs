using System.Net.Http;

namespace Softeq.XToolkit.Remote.Api
{
    public interface IApiServiceFactory
    {
        TApiService Create<TApiService>(HttpClient httpClient);
    }
}
