// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteServices.GitHub.Dtos;

namespace RemoteServices.GitHub
{
    [Headers("Authorization: token")]
    public interface IGitHubApiService
    {
        [Get("/users/{user}")]
        Task<UserDetailsResponse> GetUser(string user, CancellationToken cancellationToken);

        [Get("/users/{user}/followers")]
        Task<IList<UserResponse>> GetUserFollowers(string user, CancellationToken cancellationToken);
    }
}
