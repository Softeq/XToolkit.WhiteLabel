using Refit;

namespace RemoteServices.Auth.Dtos
{
    public class RefreshTokenRequest : AuthRequest
    {
        public RefreshTokenRequest()
        {
            GrantType = AuthConsts.GrantTypeRefreshToken;
        }

        [AliasAs("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
