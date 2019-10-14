using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp
{
    public class DataService
    {
        private readonly IRemoteService<IPhotosApiService> _remoteService;
        private readonly ILogger _logger;

        public DataService(
            IRemoteServiceFactory remoteServiceFactory,
            ILogger logger)
        {
            var httpClient = new HttpClientBuilder("https://jsonplaceholder.typicode.com")
                .WithLogger(logger);

            _remoteService = remoteServiceFactory.Create<IPhotosApiService>(httpClient);
            _logger = logger;
        }

        public async Task<string> GetDataAsync(CancellationToken cancellationToken)
        {
            // fatal api error? - on refit side
            // connectivity & fatal network error - on consumer side
            // cancellation - done

            // auth? - need refit integration
            // mapper
            // integrate with Fusillade - for priorities

            _logger.Debug("========= Begin =========");

            try
            {
                var result = await _remoteService.Execute(
                    (service, ct) => service.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        Priority = Priority.Background,
                        RetryCount = 2,
                        Timeout = 2,
                        CancellationToken = cancellationToken
                    }).ConfigureAwait(false);

                return $"Done. Count: {result.Count()}";
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
