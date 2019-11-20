using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote.Api
{
    public class RefitApiServiceFactory : IApiServiceFactory
    {
        public TApiService CreateService<TApiService>(HttpClient httpClient)
        {
            return RestService.For<TApiService>(httpClient); // new RefitSettings()
        }
    }
}
