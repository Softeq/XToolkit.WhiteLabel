// Developed for [customer name] by Softeq Development Corporation
// http://www.softeq.com
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
            // https://self-signed.badssl.com/
            var httpClientBuilder = new HttpClientBuilder("https://expired.badssl.com")
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
