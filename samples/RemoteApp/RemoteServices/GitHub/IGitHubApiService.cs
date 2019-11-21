using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteServices.GitHub.Dtos;

namespace RemoteServices.GitHub
{
    public interface IGitHubApiService
    {
        [Get("/users/{user}")]
        Task<UserDto> GetUser(string user, CancellationToken cancellationToken);
    }
}
