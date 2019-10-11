using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote;

namespace RemoteApp
{
    public class DataService
    {
        private readonly IRemoteService<IApiService> _remoteService;

        public DataService(
            IRemoteServiceFactory remoteServiceFactory)
        {
            var httpClient = new HttpClientBuilder("https://jsonplaceholder.typicode.com")
                //.WithLogger(new MyLogger())
                //.WithDefaultHeaders(new List<string>())
                .Build();

            _remoteService = remoteServiceFactory.Create<IApiService>(httpClient);
        }

        public async Task GetDataAsync()
        {
            try
            {
                // connectivity?
                // fatal api error?
                // fatal request error?
                // auth?
                // cancellation?

                var cts = new CancellationTokenSource();

                var result = await _remoteService.Execute(
                    service => service.GetAllPhotosAsync(cts.Token),
                    new RequestOptions
                    {
                        Priority = Priority.Background,
                        RetryCount = 5,
                        Timeout = 5000
                    }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
