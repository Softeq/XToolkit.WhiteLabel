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
        ///     Correct order: NativeHandler) AuthHandler) DiagnosticHandler.
        /// </remarks>
        /// <param name="delegatingHandler">Handler.</param>
        /// <returns>Current implementation of <see cref="IHttpClientBuilder"/>.</returns>
        IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler);

        /// <summary>
        ///     Builds <see cref="T:System.Net.Http.HttpClient"/> object, based on <see cref="IHttpClientBuilder"/> implementation.
        /// </summary>
        /// <returns>Returns <see cref="T:System.Net.Http.HttpClient"/> object.</returns>
        HttpClient Build();
    }
}
