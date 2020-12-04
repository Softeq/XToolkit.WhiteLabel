// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.Auth.Dtos;
using Playground.RemoteData.Auth.Models;
using Refit;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.RemoteData.Auth
{
    public interface IAuthRemoteService
    {
        Task<TokenResult> LoginAsync(string login, string password, CancellationToken cancellationToken);

        Task<TokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }

    public class AuthRemoteService : IAuthRemoteService
    {
        private readonly IRemoteService<IAuthApiService> _remoteService;
        private readonly AuthApiConfig _apiConfig;

        public AuthRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager,
            AuthApiConfig apiConfig)
        {
            var logger = logManager.GetLogger<AuthRemoteService>();
            var httpClient = httpClientFactory.CreateClient(apiConfig.BaseUrl, logger);

            _remoteService = remoteServiceFactory.Create<IAuthApiService>(httpClient);
            _apiConfig = apiConfig;
        }

        public async Task<TokenResult> LoginAsync(string login, string password, CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                UserName = login,
                Password = password,
                ClientId = _apiConfig.ClientId,
                ClientSecret = _apiConfig.ClientSecret
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
                ClientId = _apiConfig.ClientId,
                ClientSecret = _apiConfig.ClientSecret
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
