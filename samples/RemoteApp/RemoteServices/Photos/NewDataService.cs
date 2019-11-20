using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Executor;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Photos
{


    public class NewRemoteData<TApiService> : IRemoteService<TApiService>
    {
        private readonly TApiService _apiService;
        private readonly IExecutorBuilderFactory _executorFactory;

        public NewRemoteData(TApiService apiService,
            IExecutorBuilderFactory executorFactory)
        {
            _apiService = apiService;
            _executorFactory = executorFactory;
        }

        public async Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null)
        {
            options = options ?? RequestOptions.GetDefaultOptions();

            var executor = _executorFactory
                .Create<TResult>()
                .WithRetry(options.RetryCount, options.ShouldRetry)
                .WithTimeout(options.Timeout)
                .Build();

            return await executor
                .ExecuteAsync(ct => operation(_apiService, ct), options.CancellationToken)
                .ConfigureAwait(false);
        }
    }




    // YP: simple factoy for create HttpClient
    // Responsibility:
    // - return configured HttpClient
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
        private readonly NewRemoteData<IPhotosApiService> _remoteData;

        public NewDataService(Lazy<IPhotosApiService> photosApiService)
        {
            _photosApiService = photosApiService;

            _remoteData = new NewRemoteData<IPhotosApiService>(_photosApiService.Value, new PollyExecutorBuilderFactory());
        }

        public async Task GetAllPhotosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _remoteData.MakeRequest(
                    (s, ct) => s.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });
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
