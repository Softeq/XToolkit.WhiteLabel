using System.Threading;
using System.Threading.Tasks;
using RemoteServices.Profile.Models;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Profile
{
    public class ProfileRemoteService
    {
        private readonly IRemoteService<IProfileApiService> _remoteService;

        public ProfileRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            ISessionContext sessionContext,
            string baseUrl,
            ILogger logger)
        {
            var httpClientBuilder = new HttpClientBuilder(baseUrl)
                .WithLogger(logger)
                .WithSessionContext(sessionContext);

            _remoteService = remoteServiceFactory.Create<IProfileApiService>(httpClientBuilder);
        }

        public async Task<ProfileResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var requestTask = _remoteService.MakeRequest(
                (service, ct) => service.Profile(ct),
                new RequestOptions
                {
                    Priority = RequestPriority.UserInitiated,
                    CancellationToken = cancellationToken
                });

            var result = await requestTask.ConfigureAwait(false);

            return Mapper.Map(result);
        }
    }
}
