using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote.Api
{
    /// <summary>
    ///     Returns Refit API service implementation.
    /// </summary>
    public class RefitApiServiceFactory : IApiServiceFactory
    {
        public TApiService CreateService<TApiService>(HttpClient httpClient)
        {
            return RestService.For<TApiService>(httpClient);
        }
    }
}
