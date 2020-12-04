// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.Profile.Models;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.RemoteData.Profile
{
    public class ProfileRemoteService
    {
        private readonly IRemoteService<IProfileApiService> _remoteService;

        public ProfileRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ISessionContext sessionContext,
            ILogManager logManager,
            ProfileApiConfig apiConfig)
        {
            var logger = logManager.GetLogger<ProfileRemoteService>();

            var httpClient = httpClientFactory.CreateAuthClient(apiConfig.BaseUrl, sessionContext, logger);

            _remoteService = remoteServiceFactory.Create<IProfileApiService>(httpClient);
        }

        public async Task<ProfileResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var requestTask = _remoteService.MakeRequest(
                (service, ct) => service.Profile(ct),
                new RequestOptions
                {
                    CancellationToken = cancellationToken
                });

            var result = await requestTask.ConfigureAwait(false);

            return Mapper.Map(result);
        }
    }
}
