using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteApp.Services.Profile.Dtos;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp.Services.Profile
{
    [Headers(Header.BearerAuth)]
    public interface IProfileApiService
    {
        [Get("/me/profile")]
        Task<ProfileResponse> Profile(CancellationToken cancellationToken);
    }
}
