using System;
using System.Threading;
using System.Threading.Tasks;
using RemoteApp.Services.Auth.Models;
using Softeq.XToolkit.Common.Logger;

namespace RemoteApp.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginStatus> LoginAsync(string login, string password, CancellationToken cancellationToken);
    }

    public class AuthService : IAuthService
    {
        private readonly AuthRemoteService _remoteService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;

        public AuthService(
            AuthRemoteService remoteService,
            ILogger logger)
        {
            _logger = logger;
            _remoteService = remoteService;
            _tokenManager = new InMemoryTokenManager();
        }

        public async Task<LoginStatus> LoginAsync(string login, string password, CancellationToken cancellationToken)
        {
            try
            {
                await _tokenManager.ResetTokensAsync();

                var result = await _remoteService.LoginAsync(login, password, cancellationToken).ConfigureAwait(false);

                await _tokenManager.SaveAsync(result.AccessToken, result.RefreshToken);
            }
            catch (AuthException ex)
            {
                return Map(ex.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return LoginStatus.Failed;
            }

            return LoginStatus.Successful;
        }

        private static LoginStatus Map(ErrorResult error)
        {
            if (error == null)
            {
                return LoginStatus.Undefined;
            }

            switch (error.ErrorCode)
            {
                case ErrorCodes.UserNotFound:
                    return LoginStatus.EmailOrPasswordIncorrect;
                case ErrorCodes.EmailIsNotConfirmed:
                    return LoginStatus.EmailNotConfirmed;
                default:
                    return LoginStatus.Failed;
            }
        }
    }
}
