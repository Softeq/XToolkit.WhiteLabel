using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    public class RefitHttpClientBuilder : HttpClientBuilder
    {
        private ISessionContext _sessionContext;

        public RefitHttpClientBuilder(string baseUrl) : base(baseUrl)
        {
        }

        public IHttpClientBuilder WithSessionContext(ISessionContext sessionContext)
        {
            _sessionContext = sessionContext;
            return this;
        }

        protected override HttpClient CreateHttpClient(HttpMessageHandler _)
        {
            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = GetHttpMessageHandler,
            };

            if (_sessionContext != null)
            {
                settings.AuthorizationHeaderValueGetter = () => Task.FromResult(_sessionContext.AccessToken);
            }

            return RestService.CreateHttpClient(BaseUrl, settings);

            // TODO YP: reimplement refit client for support (auth & diagnostic & priority manage via HttpMessageHandlers
        }
    }
}
