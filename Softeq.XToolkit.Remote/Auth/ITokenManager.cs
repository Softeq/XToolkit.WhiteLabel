using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    public interface ITokenManager
    {
        string Token { get; }
        string RefreshToken { get; }

        Task SaveAsync(string accessToken, string refreshToken);
        Task ResetTokensAsync();
    }
}
