using System;
using System.Threading;
using System.Threading.Tasks;
using RemoteServices.Auth.Models;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace RemoteServices.Auth
{
    public interface IAuthService
    {
        Task<LoginStatus> LoginAsync(string login, string password, CancellationToken cancellationToken);

        Task<RefreshTokenStatus> RefreshTokenAsync(CancellationToken cancellationToken);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthRemoteService _remoteService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;

        public AuthService(
            IAuthRemoteService remoteService,
            ILogger logger,
            ITokenManager tokenManager)
        {
            _logger = logger;
            _remoteService = remoteService;
            _tokenManager = tokenManager;
        }

        public async Task<LoginStatus> LoginAsync(string login, string password, CancellationToken cancellationToken)
        {
            try
            {
                await _tokenManager.ResetTokensAsync().ConfigureAwait(false);

                var result = await _remoteService.LoginAsync(login, password, cancellationToken).ConfigureAwait(false);

                await _tokenManager.SaveAsync(result.AccessToken, result.RefreshToken).ConfigureAwait(false);
            }
            catch (AuthException ex)
            {
                return Mapper.Map(ex.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return LoginStatus.Failed;
            }

            return LoginStatus.Successful;
        }

        public async Task<RefreshTokenStatus> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = _tokenManager.RefreshToken;
                var result = await _remoteService.RefreshTokenAsync(refreshToken, cancellationToken).ConfigureAwait(false);

                await _tokenManager.SaveAsync(result.AccessToken, result.RefreshToken).ConfigureAwait(false);

                return RefreshTokenStatus.Successful;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return RefreshTokenStatus.Failed;
            }
        }
    }
}
