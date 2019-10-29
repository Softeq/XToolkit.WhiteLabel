using Refit;

namespace RemoteApp.Services.Auth.Dtos
{
    public class LoginRequest : AuthRequest
    {
        public LoginRequest()
        {
            GrantType = AuthConsts.GrantTypePassword;
            Scope = AuthConsts.ScopeApiOfflineAccess;
        }

        [AliasAs("username")]
        public string UserName { get; set; }

        [AliasAs("password")]
        public string Password { get; set; }

        [AliasAs("scope")]
        public string Scope { get; set; }
    }
}
