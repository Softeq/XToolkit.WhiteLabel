using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;

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
                .WithLogger(logger)
                .Build();

            _remoteService = remoteServiceFactory.Create<IPhotosApiService>(httpClient);
            _logger = logger;
        }

        public async Task<string> GetDataAsync()
        {
            _logger.Debug("============== Start request");
            try
            {
                // connectivity?
                // fatal api error?
                // fatal request error?
                // auth?
                // cancellation?
                // mapper

                var cts = new CancellationTokenSource();

                var result = await _remoteService.Execute(
                    service => service.GetAllPhotosAsync(cts.Token),
                    new RequestOptions
                    {
                        Priority = Priority.Background,
                        RetryCount = 5,
                        Timeout = 5000
                    }).ConfigureAwait(false);

                return $"Done. Count: {result.Count()}";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                _logger.Debug("============== End request");
            }
        }
    }
}
