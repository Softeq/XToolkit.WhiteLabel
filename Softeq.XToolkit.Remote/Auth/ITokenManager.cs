// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    /// <summary>
    ///     Default contract for token management.
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        ///    Gets access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        ///     Gets refresh token.
        /// </summary>
        string RefreshToken { get; }

        /// <summary>
        ///     Saves token.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="refreshToken">Refresh token.</param>
        /// <returns>Task.</returns>
        Task SaveAsync(string accessToken, string refreshToken);

        /// <summary>
        ///     Removes tokens.
        /// </summary>
        /// <returns>Task.</returns>
        Task ResetTokensAsync();
    }
}
