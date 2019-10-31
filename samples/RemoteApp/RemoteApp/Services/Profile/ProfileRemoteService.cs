using System.Threading;
using System.Threading.Tasks;
using RemoteApp.Services.Profile.Models;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp.Services.Profile
{
    public class ProfileRemoteService
    {
        private readonly IRemoteService<IProfileApiService> _remoteService;

        public ProfileRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            ISessionContext sessionContext,
            string baseUrl)
        {
            var httpClientBuilder = new RefitHttpClientBuilder(baseUrl)
                .WithSessionContext(sessionContext);

            _remoteService = remoteServiceFactory.CreateWithAuth<IProfileApiService>(httpClientBuilder, sessionContext);
        }

        public async Task<ProfileResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var requestTask = _remoteService.MakeRequest(
                (service, ct) => service.Profile(ct),
                new RequestOptions
                {
                    Timeout = 15,
                    CancellationToken = cancellationToken
                });

            var result = await requestTask.ConfigureAwait(false);

            return Mapper.Map(result);
        }
    }
}
