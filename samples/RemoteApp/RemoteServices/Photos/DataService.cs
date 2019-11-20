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
            ILogger logger)
        {
            var httpClientBuilder = new HttpClientBuilder("https://jsonplaceholder.typicode.com")
                .WithLogger(logger);

            _remoteService = remoteServiceFactory.Create<IPhotosApiService>(httpClientBuilder);
            _logger = logger;
        }

        public async Task<string> GetDataAsync(CancellationToken cancellationToken)
        {
            // TODO YP:
            // mapper - need default, declaration on consumer side

            // Bugs:
            // throttling & retry - logs many results

            _logger.Debug("========= Begin =========");

            try
            {
                var result = await _remoteService.MakeRequest(
                    (service, ct) => service.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        Priority = RequestPriority.UserInitiated,
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
