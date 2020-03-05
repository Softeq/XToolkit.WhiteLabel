// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using RemoteServices.GitHub.Models;

namespace RemoteServices.GitHub
{
    public class GitHubRemoteService
    {
        private const string ApiUrl = "https://api.github.com";

        private readonly IRemoteService<IGitHubApiService> _remoteService;
        private readonly ILogger _logger;

        public GitHubRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager,
            GitHubSessionContext sessionContext)
        {
            _logger = logManager.GetLogger<GitHubRemoteService>();

            var httpClient = httpClientFactory.CreateAuthClient(ApiUrl, sessionContext, _logger);

            _remoteService = remoteServiceFactory.Create<IGitHubApiService>(httpClient);
        }

        public async Task<User> GetUserAsync(string login, CancellationToken cancellationToken)
        {
            var dto = await _remoteService.SafeRequest(
                (s, ct) => s.GetUser(login, ct),
                cancellationToken,
                _logger);

            return Mapper.Map(dto);
        }

        public async Task<IList<User>> GetUserFollowersAsync(string login, CancellationToken cancellationToken)
        {
            var dtos = await _remoteService.SafeRequest(
                (s, ct) => s.GetUserFollowers(login, ct),
                cancellationToken,
                _logger);

            return Mapper.MapAll(dtos, Mapper.Map);
        }
    }
}
