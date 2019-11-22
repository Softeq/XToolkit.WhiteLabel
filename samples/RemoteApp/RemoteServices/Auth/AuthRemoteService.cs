using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteServices.Auth.Dtos;
using RemoteServices.Auth.Models;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Auth
{
    public interface IAuthRemoteService
    {
        Task<TokenResult> LoginAsync(string login, string password, CancellationToken cancellationToken);

        Task<TokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }

    public class AuthRemoteService : IAuthRemoteService
    {
        private readonly IRemoteService<IAuthApiService> _remoteService;
        private readonly AuthConfig _config;

        public AuthRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager,
            AuthConfig config)
        {
            var logger = logManager.GetLogger<AuthRemoteService>();
            var httpClient = httpClientFactory.CreateClient(config.BaseUrl, logger);

            _remoteService = remoteServiceFactory.Create<IAuthApiService>(httpClient);
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
                .MakeRequest(
                    (service, ct) => service.Login(request, ct),
                    new RequestOptions
                    {
                        RetryCount = 2,
                        ShouldRetry = ex => !(ex is ApiException),
                        CancellationToken = cancellationToken
                    });

            var result = await ExecuteWithMapExceptions(requestTask).ConfigureAwait(false);

            return Mapper.Map(result);
        }

        public async Task<TokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var request = new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret
            };

            var requestTask = _remoteService
                .MakeRequest(
                    (service, ct) => service.RefreshToken(request, ct),
                    new RequestOptions
                    {
                        RetryCount = 3,
                        ShouldRetry = ex => !(ex is ApiException),
                        Timeout = 2,
                        CancellationToken = cancellationToken
                    });

            var result = await ExecuteWithMapExceptions(requestTask).ConfigureAwait(false);

            return Mapper.Map(result);
        }

        private static async Task<TResult> ExecuteWithMapExceptions<TResult>(Task<TResult> requestTask)
        {
            try
            {
                return await requestTask.ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                throw await Mapper.MapAsync(ex).ConfigureAwait(false);
            }
        }
    }
}
