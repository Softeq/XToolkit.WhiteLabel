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
            var config = new RemoteServiceConfig("https://jsonplaceholder.typicode.com/");

            _remoteService = remoteServiceFactory.Create<IApiService>(config);
        }

        public async Task GetDataAsync()
        {
            var a = await _remoteService.Execute(service => service.GetAllPhotosAsync());
        }
    }
}
