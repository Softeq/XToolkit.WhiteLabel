using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;

namespace RemoteServices.Photos
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

        public async Task CheckStatusCode(int statusCode, CancellationToken cancellationToken)
        {

//            var a = await _remoteData.SafeRequest(
//                (s, ct) => s.CheckStatus(statusCode, ct),
//                cancellationToken,
//                _logger);
        }
    }

    public interface IHttpBinApiService
    {
        [Get("/status/{statusCode}")]
        Task CheckStatus(int statusCode, CancellationToken cancellationToken);
    }
}
