// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;

namespace RemoteServices.Auth.Dtos
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
