// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;

namespace Softeq.XToolkit.Remote.Client
{
    public static class ClientExtensions
    {
        /// <summary>
        ///    Configures <paramref name="httpClientBuilder"/> to handle Authorization header with Access and Refresh tokens.
        /// </summary>
        /// <param name="httpClientBuilder">Target builder.</param>
        /// <param name="logger">Logger instance.</param>
        /// <returns>Modified builder.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="httpClientBuilder"/> parameter is <see langword="null"/> or
        ///     the <paramref name="logger"/> parameter is <see langword="null"/>.
        /// </exception>
        public static IHttpClientBuilder WithLogger(
            this IHttpClientBuilder httpClientBuilder,
            ILogger logger)
        {
            if (httpClientBuilder == null)
            {
                throw new ArgumentNullException(nameof(httpClientBuilder));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var handler = new HttpDiagnosticsHandler(logger);

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
