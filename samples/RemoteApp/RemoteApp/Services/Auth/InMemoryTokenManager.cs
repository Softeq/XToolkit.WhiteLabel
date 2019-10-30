using System.Threading.Tasks;

namespace RemoteApp.Services.Auth
{
    public class InMemoryTokenManager : ITokenManager
    {
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }

        public Task SaveAsync(string accessToken, string refreshToken)
        {
            Token = accessToken;
            RefreshToken = refreshToken;

            return Task.CompletedTask;
        }

        public Task ResetTokensAsync()
        {
            Token = string.Empty;
            RefreshToken = string.Empty;

            return Task.CompletedTask;
        }
    }
}