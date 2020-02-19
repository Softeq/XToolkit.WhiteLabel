// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace RemoteServices.Test
{
    public interface IHttpBinApiService
    {
        [Get("/status/{statusCode}")]
        Task CheckStatus(int statusCode, CancellationToken cancellationToken);
    }
}
