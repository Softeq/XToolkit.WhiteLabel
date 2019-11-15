using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Tests
{
    public class TestRemoteService
    {
        private readonly IRemoteService<ISslApiService> _remoteService;
        private readonly ILogger _logger;

        public TestRemoteService(ILogger logger)
        {
            //var url = "https://self-signed.badssl.com";
            var url = "https://expired.badssl.com";

            var httpClientBuilder = new HttpClientBuilder(url)
                .WithLogger(logger);

            _remoteService = new RemoteServiceFactory().Create<ISslApiService>(httpClientBuilder);
            _logger = logger;
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
