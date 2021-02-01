// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.GitHub.Models;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;

namespace Playground.RemoteData.GitHub
{
    public class GitHubRemoteService
    {
        private const string ApiUrl = "https://api.github.com";

        private readonly Lazy<IRemoteService<IGitHubApiService>> _remoteServiceLazy;

        public GitHubRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager,
            GitHubSessionContext sessionContext)
        {
            _remoteServiceLazy = new Lazy<IRemoteService<IGitHubApiService>>(() =>
            {
                var logger = logManager.GetLogger<GitHubRemoteService>();
                var httpClient = httpClientFactory.CreateAuthClient(ApiUrl, sessionContext, logger);
                return remoteServiceFactory.Create<IGitHubApiService>(httpClient);
            });
        }

        public async Task<User> GetUserAsync(string login, CancellationToken cancellationToken)
        {
            var dto = await _remoteServiceLazy.Value.SafeRequest(
                (s, ct) => s.GetUser(login, ct),
                cancellationToken);

            return Mapper.Map(dto);
        }

        public async Task<IList<User>> GetUserFollowersAsync(string login, CancellationToken cancellationToken)
        {
            var dtos = await _remoteServiceLazy.Value.SafeRequest(
                (s, ct) => s.GetUserFollowers(login, ct),
                cancellationToken);

            return Mapper.MapAll(dtos, Mapper.Map);
        }
    }
}
