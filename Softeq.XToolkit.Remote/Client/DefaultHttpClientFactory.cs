// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     Default factory to create <see cref="T:System.Net.Http.HttpClient"/> instances
    ///     with custom configuration.
    /// </summary>
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        /// <inheritdoc />
        public virtual HttpClient CreateClient(string baseUrl, ILogger? logger = null)
        {
            var builder = new HttpClientBuilder().WithBaseUrl(baseUrl);

            if (logger != null)
            {
                builder.WithLogger(logger);
            }

            return builder.Build();
        }

        /// <inheritdoc />
        public virtual HttpClient CreateAuthClient(
            string baseUrl,
            ISessionContext sessionContext,
            ILogger? logger = null)
        {
            var builder = new HttpClientBuilder().WithBaseUrl(baseUrl);

            if (logger != null)
            {
                builder.WithLogger(logger);
            }

            builder.WithSessionContext(sessionContext);

            return builder.Build();
        }
    }
}
