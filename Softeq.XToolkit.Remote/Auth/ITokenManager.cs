// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    public interface ITokenManager
    {
        string AccessToken { get; }
        string RefreshToken { get; }

        Task SaveAsync(string accessToken, string refreshToken);
        Task ResetTokensAsync();
    }
}
