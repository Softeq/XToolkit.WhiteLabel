using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace RemoteServices.Photos
{
    public interface IHttpBinApiService
    {
        [Get("/status/{statusCode}")]
        Task CheckStatus(int statusCode, CancellationToken cancellationToken);
    }
}
