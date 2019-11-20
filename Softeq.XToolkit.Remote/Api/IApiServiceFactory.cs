using System.Net.Http;

namespace Softeq.XToolkit.Remote.Api
{
    // YP: simple factory for return api service implementation, refit or etc.
    // Responsibility:
    // - return api service implementation;
    public interface IApiServiceFactory
    {
        TApiService CreateService<TApiService>(HttpClient httpClient);
    }
}
