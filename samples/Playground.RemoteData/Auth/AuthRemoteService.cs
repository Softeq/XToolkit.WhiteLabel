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
#pragma warning disable SA1649
    public interface IAuthRemoteService
#pragma warning restore SA1649
    {
        Task<TokenResult> LoginAsync(string login, string password, CancellationToken cancellationToken);

        Task<TokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }

    public class AuthRemoteService : IAuthRemoteService
    {
        private readonly Lazy<IRemoteService<IAuthApiService>> _remoteServiceLazy;
        private readonly AuthApiConfig _apiConfig;

        public AuthRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager,
            AuthApiConfig apiConfig)
        {
            _apiConfig = apiConfig;

            _remoteServiceLazy = new Lazy<IRemoteService<IAuthApiService>>(() =>
            {
                var logger = logManager.GetLogger<AuthRemoteService>();
                var httpClient = httpClientFactory.CreateClient(apiConfig.BaseUrl, logger);
                return remoteServiceFactory.Create<IAuthApiService>(httpClient);
            });
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

            var requestTask = _remoteServiceLazy.Value
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

            var requestTask = _remoteServiceLazy.Value
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
