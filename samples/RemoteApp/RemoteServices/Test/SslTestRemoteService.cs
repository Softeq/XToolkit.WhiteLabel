using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Ssl
{
    public class SslTestRemoteService
    {
        private readonly IRemoteService<ISslApiService> _remoteService;
        private readonly ILogger _logger;

        public SslTestRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            ILogManager logManager,
            HttpMessageHandler customHttpMessageHandler)
        {
            _logger = logManager.GetLogger<SslTestRemoteService>();

            //var url = "https://self-signed.badssl.com";
            var url = "https://expired.badssl.com";

            var customHttpMessageHandlerBuilder = new DefaultHttpMessageHandlerBuilder(customHttpMessageHandler);
            var httpClientBuilder = new HttpClientBuilder(url, customHttpMessageHandlerBuilder)
                .WithLogger(_logger)
                .Build();

            _remoteService = remoteServiceFactory.Create<ISslApiService>(httpClientBuilder);
        }

        public async Task<string> CheckExpiredSslAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _remoteService.MakeRequest(
                    (s, ct) => s.GetHome(ct),
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return ex.Message;
            }
        }
    }
}
