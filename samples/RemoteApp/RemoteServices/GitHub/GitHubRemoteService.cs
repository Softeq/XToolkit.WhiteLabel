using System;
using System.Threading;
using System.Threading.Tasks;
using RemoteServices.GitHub.Models;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.GitHub
{
    public class GitHubRemoteService
    {
        private const string ApiUrl = "https://api.github.com";

        private readonly IRemoteService<IGitHubApiService> _remoteService;

        public GitHubRemoteService(
            IRemoteServiceFactory remoteServiceFactory)
        {
            _remoteService = remoteServiceFactory.Create<IGitHubApiService>(ApiUrl);
        }

        public async Task<User> GetUserAsync(string login, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _remoteService.MakeRequest(
                    (s, ct) => s.GetUser(login, ct),
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });

                return Mapper.Map(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
