// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;

namespace RemoteServices.Test
{
    public class HttpBinRemoteService
    {
        private const string ApiUrl = "https://httpbin.org";

        private readonly IRemoteService<IHttpBinApiService> _remoteData;
        private readonly ILogger _logger;

        public HttpBinRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            ILogManager logManager)
        {
            _logger = logManager.GetLogger<HttpBinRemoteService>();
            _remoteData = remoteServiceFactory.Create<IHttpBinApiService>(ApiUrl);
        }

        public Task CheckStatusCode(int statusCode, CancellationToken cancellationToken)
        {
            return _remoteData.SafeRequest(
                (s, ct) => s.CheckStatus(statusCode, ct),
                cancellationToken,
                _logger);
        }
    }
}
