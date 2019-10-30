using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace Softeq.XToolkit.Remote.Client
{
    public class RefitHttpClientBuilder : HttpClientBuilder
    {
        public RefitHttpClientBuilder(string baseUrl) : base(baseUrl)
        {
        }

        public override HttpClient Build()
        {
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = GetHttpMessageHandler,
                AuthorizationHeaderValueGetter = () =>
                {
                    return Task.FromResult("token temp");
                }
            };

            return RestService.CreateHttpClient(BaseUrl, settings);
        }
    }
}
