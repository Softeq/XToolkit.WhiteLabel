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
        public TApiService CreateService<TApiService>(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            return RestService.For<TApiService>(httpClient);
        }
    }
}
