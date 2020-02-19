// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     Provides methods for create configured <see cref="HttpClient"/> object.
    /// </summary>
    public interface IHttpClientBuilder
    {
        /// <summary>
        ///     Adds a delegate that will be used to create an additional message handler for a <see cref="HttpClient" />.
        /// </summary>
        /// <remarks>
        ///     Correct order:
        //          NativeHandler) AuthHandler) DiagnosticHandler
        /// </remarks>
        /// <param name="delegatingHandler"></param>
        /// <returns></returns>
        IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler);

        /// <summary>
        ///     Builds <see cref="HttpClient"/> object, based on <see cref="IHttpClientBuilder"/> implementation.
        /// </summary>
        /// <returns>Returns <see cref="HttpClient"/> object.</returns>
        HttpClient Build();
    }
}
