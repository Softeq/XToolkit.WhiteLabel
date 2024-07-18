// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote.Api
{
    /// <summary>
    ///     Returns Refit API service implementation.
    /// </summary>
    public class RefitApiServiceFactory : IApiServiceFactory
    {
        private readonly RefitSettings? _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RefitApiServiceFactory"/> class.
        /// </summary>
        /// <param name="settings">Refit settings.</param>
        public RefitApiServiceFactory(RefitSettings? settings = null)
        {
            _settings = settings;
        }

        /// <inheritdoc />
        public TApiService CreateService<TApiService>(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            return RestService.For<TApiService>(httpClient, _settings);
        }
    }
}
