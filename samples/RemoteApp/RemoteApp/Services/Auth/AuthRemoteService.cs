using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteApp.Services.Auth.Dtos;
using RemoteApp.Services.Auth.Models;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp.Services.Auth
{
    public class AuthRemoteService
    {
        private readonly IRemoteService<IAuthApiService> _remoteService;
        private readonly AuthConfig _config;

        public AuthRemoteService(IRemoteServiceFactory remoteServiceFactory, AuthConfig config)
        {
            var httpClientBuilder = new HttpClientBuilder(config.BaseUrl);

            _remoteService = remoteServiceFactory.Create<IAuthApiService>(httpClientBuilder);
            _config = config;
        }

        public async Task<TokenResult> LoginAsync(string login, string password, CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                UserName = login,
                Password = password,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret
            };

            var requestTask = _remoteService
                .Execute(
                    (service, ct) => service.Login(request, ct),
                    new RequestOptions
                    {
                        Priority = RequestPriority.UserInitiated,
                        RetryCount = 2,
                        ShouldRetry = ex => !(ex is ApiException),
                        Timeout = 2,
                        CancellationToken = cancellationToken
                    });

            try
            {
                var response = await requestTask.ConfigureAwait(false);

                return new TokenResult
                {
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken
                };
            }
            catch (ApiException ex)
            {
                throw await Mapper.MapAsync(ex).ConfigureAwait(false);
            }
        }

        public async Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var request = new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret
            };

            var response = await _remoteService
                .Execute((service, ct) => service.RefreshToken(request, ct))
                .ConfigureAwait(false);
        }
    }
}
