// Developed by Softeq Development Corporation
// http://www.softeq.com

using Refit;

namespace RemoteServices.Auth.Dtos
{
    public class LoginRequest : AuthRequest
    {
        public LoginRequest()
        {
            GrantType = AuthConsts.GrantTypePassword;
            Scope = AuthConsts.ScopeApiOfflineAccess;
        }

        [AliasAs("username")]
        public string UserName { get; set; } = string.Empty;

        [AliasAs("password")]
        public string Password { get; set; } = string.Empty;
    }
}
