using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote
{
    public interface IApiServiceFactory
    {
        TApiService Create<TApiService>(HttpClient httpClient);
    }

    public class RefitApiServiceFactory : IApiServiceFactory
    {
        public TApiService Create<TApiService>(HttpClient httpClient)
        {
            return RestService.For<TApiService>(httpClient);
        }
    }
}
