// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     A factory abstraction for a component that can create <see cref="T:System.Net.Http.HttpClient"/> instances
    ///     with custom configuration.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        ///     Creates and configures an <see cref="T:System.Net.Http.HttpClient"/> instance
        ///     using the configuration that corresponds to the arguments.
        /// </summary>
        /// <param name="baseUrl">The base address used when sending requests.</param>
        /// <param name="logger">Logger instance (optional).</param>
        /// <returns>New instance of <see cref="T:System.Net.Http.HttpClient"/>.</returns>
        HttpClient CreateClient(string baseUrl, ILogger? logger = null);

        /// <summary>
        ///     Creates and configures an <see cref="T:System.Net.Http.HttpClient"/> instance with authorization support.
        /// </summary>
        /// <param name="baseUrl">The base address used when sending requests.</param>
        /// <param name="sessionContext">Context to support authorization.</param>
        /// <param name="logger">Logger instance (optional).</param>
        /// <returns>New instance of <see cref="T:System.Net.Http.HttpClient"/>.</returns>
        HttpClient CreateAuthClient(string baseUrl, ISessionContext sessionContext, ILogger? logger = null);
    }
}
