using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Remote.Client;

namespace RemoteServices.Photos
{




    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient(string baseUrl);
    }

    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateHttpClient(string baseUrl)
        {
            var httpHandler = HttpHandlerBuilder.NativeHandler;

            return new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }





    // YP: simple factory for return api service implementation, refit or etc.
    // Responsibility:
    // - return api service implementation;
    public interface IApiServiceFactory
    {
        TApiService CreateService<TApiService>(HttpClient httpClient);
    }
    public class RefitApiServiceFactory : IApiServiceFactory
    {
        public TApiService CreateService<TApiService>(HttpClient httpClient)
        {
            return RestService.For<TApiService>(httpClient); // new RefitSettings()
        }
    }


    // YP: simple consumer service for make API call as simples as possible.
    // Responsibility:
    // - Fetch data from API;
    // - Catch remote exceptions;
    // - Map data & error DTOs to the app models;
    public class NewDataService
    {
        private readonly Lazy<IPhotosApiService> _photosApiService;

        public NewDataService(Lazy<IPhotosApiService> photosApiService)
        {
            _photosApiService = photosApiService;
        }

        public async Task GetAllPhotosAsync()
        {
            try
            {
                var data = await _photosApiService.Value.GetAllPhotosAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                // parse to error
            }
            catch (Exception ex)
            {
                // on next
                throw;
            }
        }
    }
}
