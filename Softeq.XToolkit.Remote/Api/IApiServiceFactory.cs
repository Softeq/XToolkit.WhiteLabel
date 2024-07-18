// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;

namespace Softeq.XToolkit.Remote.Api
{
    /// <summary>
    ///     Returns API service implementation.
    /// </summary>
    public interface IApiServiceFactory
    {
        /// <summary>
        ///     Creates API service implementation.
        /// </summary>
        /// <param name="httpClient">Configured instance of HttpClient.</param>
        /// <typeparam name="TApiService">Type of API service.</typeparam>
        /// <returns>API service implementation.</returns>
        TApiService CreateService<TApiService>(HttpClient httpClient);
    }
}
