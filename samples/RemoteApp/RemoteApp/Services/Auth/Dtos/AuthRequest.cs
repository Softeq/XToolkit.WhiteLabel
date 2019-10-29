using Refit;

namespace RemoteApp.Services.Auth.Dtos
{
    public class AuthRequest
    {
        [AliasAs("client_id")]
        public string ClientId { get; set; }

        [AliasAs("client_secret")]
        public string ClientSecret { get; set; }

        [AliasAs("grant_type")]
        public string GrantType { get; set; }
    }
}
