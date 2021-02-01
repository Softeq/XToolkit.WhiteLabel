// Developed by Softeq Development Corporation
// http://www.softeq.com

using Refit;

namespace Playground.RemoteData.Auth.Dtos
{
    public class RefreshTokenRequest : AuthRequest
    {
        public RefreshTokenRequest()
        {
            GrantType = AuthConsts.GrantTypeRefreshToken;
            Scope = AuthConsts.ScopeApiOfflineAccess;
        }

        [AliasAs("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
