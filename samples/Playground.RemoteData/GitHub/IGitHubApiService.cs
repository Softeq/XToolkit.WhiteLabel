// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.GitHub.Dtos;
using Refit;

namespace Playground.RemoteData.GitHub
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
