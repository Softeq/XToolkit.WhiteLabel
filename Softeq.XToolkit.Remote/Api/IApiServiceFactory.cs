using System.Net.Http;

namespace Softeq.XToolkit.Remote.Api
{
    /// <summary>
    ///     Returns API service implementation.
    /// </summary>
    public interface IApiServiceFactory
    {
        TApiService CreateService<TApiService>(HttpClient httpClient);
    }
}
