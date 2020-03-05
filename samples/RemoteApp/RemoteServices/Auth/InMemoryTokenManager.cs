// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth;

namespace RemoteServices.Auth
{
    public class InMemoryTokenManager : ITokenManager
    {
        public string AccessToken { get; private set; }= string.Empty;
        public string RefreshToken { get; private set; }= string.Empty;

        public Task SaveAsync(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;

            return Task.CompletedTask;
        }

        public Task ResetTokensAsync()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;

            return Task.CompletedTask;
        }
    }
}
