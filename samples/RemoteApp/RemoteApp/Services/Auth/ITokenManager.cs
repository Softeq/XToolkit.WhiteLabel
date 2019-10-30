using System.Threading.Tasks;

namespace RemoteApp.Services.Auth
{
    public interface ITokenManager
    {
        string Token { get; }
        string RefreshToken { get; }

        Task SaveAsync(string accessToken, string refreshToken);
        Task ResetTokensAsync();
    }
}