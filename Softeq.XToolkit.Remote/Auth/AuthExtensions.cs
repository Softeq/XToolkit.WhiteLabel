// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth.Handlers;
using Softeq.XToolkit.Remote.Client;

namespace Softeq.XToolkit.Remote.Auth
{
    /// <summary>
    ///     Contains extension methods for <see cref="IHttpClientBuilder"/> related to auth.
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        ///    Configures <paramref name="httpClientBuilder"/> to handle Authorization header with Access and Refresh tokens.
        /// </summary>
        /// <param name="httpClientBuilder">Target builder.</param>
        /// <param name="sessionContext">Session context instance.</param>
        /// <returns>Modified builder.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="httpClientBuilder"/> parameter is <see langword="null"/> or
        ///     the <paramref name="sessionContext"/> parameter is <see langword="null"/>.
        /// </exception>
        public static IHttpClientBuilder WithSessionContext(
            this IHttpClientBuilder httpClientBuilder,
            ISessionContext sessionContext)
        {
            if (httpClientBuilder == null)
            {
                throw new ArgumentNullException(nameof(httpClientBuilder));
            }

            if (sessionContext == null)
            {
                throw new ArgumentNullException(nameof(sessionContext));
            }

            var handler = new RefreshTokenHttpClientHandler(
                () => Task.FromResult(sessionContext.AccessToken),
                sessionContext.RefreshTokenAsync);

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
