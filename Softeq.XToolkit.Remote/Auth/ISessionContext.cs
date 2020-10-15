// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    /// <summary>
    ///     The context used in HttpClient with authorization support to store the access token, and can refresh it.
    /// </summary>
    public interface ISessionContext
    {
        /// <summary>
        ///     Gets current access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        ///    Refreshes an access token.
        /// </summary>
        /// <returns>Task.</returns>
        Task RefreshTokenAsync();
    }
}
