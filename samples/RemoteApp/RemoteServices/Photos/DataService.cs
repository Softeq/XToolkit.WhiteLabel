using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Photos
{
    // YP: simple consumer service for make API call as simples as possible.
    // Responsibility:
    // - Fetch data from API;
    // - Catch remote exceptions;
    // - Map data & error DTOs to the app models;
    public class DataService
    {
        private readonly IRemoteService<IPhotosApiService> _remoteService;
        private readonly ILogger _logger;

        public DataService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogger logger)
        {
            var httpClient = httpClientFactory.CreateSimpleClient("https://jsonplaceholder.typicode.com", logger);

            _remoteService = remoteServiceFactory.Create<IPhotosApiService>(httpClient);
            _logger = logger;
        }

        public async Task<string> GetDataAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("========= Begin =========");

            try
            {
                var result = await _remoteService.MakeRequest(
                    (service, ct) => service.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        RetryCount = 2,
                        Timeout = 2,
                        CancellationToken = cancellationToken
                    }).ConfigureAwait(false);

                return $"Done. Count: {result.Count}";
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                _logger.Debug("========== End ==========");
            }

            return "Failed. See log.";
        }
    }
}
