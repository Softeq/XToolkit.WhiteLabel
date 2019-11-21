using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace RemoteServices.Ssl
{
    public interface ISslApiService
    {
        [Get("/")]
        Task<string> GetHome(CancellationToken cancellationToken);
    }
}
