// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.Profile.Dtos;
using Refit;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.RemoteData.Profile
{
    [Headers(Header.BearerAuth)]
    public interface IProfileApiService
    {
        [Get("/me/profile")]
        Task<ProfileResponse> Profile(CancellationToken cancellationToken);
    }
}
