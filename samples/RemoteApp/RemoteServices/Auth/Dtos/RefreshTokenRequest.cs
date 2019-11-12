using Refit;

namespace RemoteServices.Auth.Dtos
{
    public class RefreshTokenRequest : AuthRequest
    {
        public RefreshTokenRequest()
        {
            GrantType = AuthConsts.GrantTypeRefreshToken;
            Scope = AuthConsts.ScopeApiOfflineAccess;
        }

        [AliasAs("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
