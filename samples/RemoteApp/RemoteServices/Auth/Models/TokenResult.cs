// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace RemoteServices.Auth.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
