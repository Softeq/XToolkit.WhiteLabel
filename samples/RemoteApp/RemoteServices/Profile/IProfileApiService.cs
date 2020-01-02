using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteServices.Profile.Dtos;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Profile
{
    [Headers(Header.BearerAuth)]
    public interface IProfileApiService
    {
        [Get("/me/profile")]
        Task<ProfileResponse> Profile(CancellationToken cancellationToken);
    }
}
