// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Playground.RemoteData.Test
{
    public interface ISslApiService
    {
        [Get("/")]
        Task<string> GetHome(CancellationToken cancellationToken);
    }
}
