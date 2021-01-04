// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
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
        private readonly Lazy<IRemoteService<IProfileApiService>> _remoteServiceLazy;

        public ProfileRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ISessionContext sessionContext,
            ILogManager logManager,
            ProfileApiConfig apiConfig)
        {
            _remoteServiceLazy = new Lazy<IRemoteService<IProfileApiService>>(() =>
            {
                var logger = logManager.GetLogger<ProfileRemoteService>();
                var httpClient = httpClientFactory.CreateAuthClient(apiConfig.BaseUrl, sessionContext, logger);
                return remoteServiceFactory.Create<IProfileApiService>(httpClient);
            });
        }

        public async Task<ProfileResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var requestTask = _remoteServiceLazy.Value.MakeRequest(
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
