// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     Provides methods for create configured <see cref="T:System.Net.Http.HttpClient"/> object.
    /// </summary>
    public interface IHttpClientBuilder
    {
        /// <summary>
        ///     Adds a delegate that will be used to create an additional message handler for a <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <remarks>
        ///     Correct order:
        ///       -> DiagnosticHandler (logs all on high-level)
        ///          -> AuthHandler (over the native level)
        ///             -> NativeHandler (low-level request/response).
        /// </remarks>
        /// <param name="delegatingHandler">Handler.</param>
        /// <returns>Current implementation of <see cref="IHttpClientBuilder"/>.</returns>
        IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler);

        /// <summary>
        ///    Set base address that is used in <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <param name="baseUrl">The base address that is used in HttpClient.</param>
        /// <returns>Current implementation of <see cref="IHttpClientBuilder"/>.</returns>
        IHttpClientBuilder WithBaseUrl(string baseUrl);

        /// <summary>
        ///     Builds <see cref="T:System.Net.Http.HttpClient"/> object, based on <see cref="IHttpClientBuilder"/> implementation.
        /// </summary>
        /// <returns>Returns <see cref="T:System.Net.Http.HttpClient"/> object.</returns>
        HttpClient Build();
    }
}
