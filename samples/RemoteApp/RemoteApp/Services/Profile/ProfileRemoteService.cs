using System;
using System.Threading;
using System.Threading.Tasks;
using RemoteApp.Services.Profile.Models;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp.Services.Profile
{
    public class ProfileRemoteService
    {
        private readonly IRemoteService<IProfileApiService> _remoteService;

        public ProfileRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            string baseUrl)
        {
            var httpClientBuilder = new HttpClientBuilder(baseUrl);
            _remoteService = remoteServiceFactory.Create<IProfileApiService>(httpClientBuilder);
        }

        public async Task<ProfileResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var requestTask = _remoteService.Execute(
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
