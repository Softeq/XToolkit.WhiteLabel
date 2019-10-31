using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth;

namespace RemoteApp.Services.Auth
{
    public class SessionContext : ISessionContext
    {
        private readonly ITokenManager _tokenManager;
        private readonly IAuthService _refreshTokenService;

        public SessionContext(
            ITokenManager tokenManager,
            IAuthService refreshTokenService)
        {
            _tokenManager = tokenManager;
            _refreshTokenService = refreshTokenService;
        }

        public string AccessToken => _tokenManager.Token;

        public Task RefreshTokenAsync() // TODO YP: support concurrency access
        {
            return _refreshTokenService.RefreshTokenAsync(CancellationToken.None);
        }
    }
}
