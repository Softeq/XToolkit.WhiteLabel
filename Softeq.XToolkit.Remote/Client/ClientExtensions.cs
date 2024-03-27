// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     Contains extension methods for <see cref="IHttpClientBuilder"/>.
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        ///    Configures <paramref name="httpClientBuilder"/> to handle Authorization header with Access and Refresh tokens.
        /// </summary>
        /// <param name="httpClientBuilder">Target builder.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="verbosity">Instance of verbosity bitmask. Default is <see cref="LogVerbosity.Unspecified"/>.</param>
        /// <returns>Modified builder.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="httpClientBuilder"/> parameter is <see langword="null"/> or
        ///     the <paramref name="logger"/> parameter is <see langword="null"/>.
        /// </exception>
        public static IHttpClientBuilder WithLogger(
            this IHttpClientBuilder httpClientBuilder,
            ILogger logger,
            LogVerbosity verbosity = LogVerbosity.Unspecified)
        {
            if (httpClientBuilder == null)
            {
                throw new ArgumentNullException(nameof(httpClientBuilder));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var handler = new HttpDiagnosticsHandler(logger, verbosity);

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
