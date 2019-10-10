using System.Threading.Tasks;
using Softeq.XToolkit.Remote;

namespace RemoteApp
{
    public static class App
    {
        public static async Task SimpleRequest()
        {
            var remoteFactory = new RestRemoteServiceFactory();
            var dataService = new DataService(remoteFactory);
            await dataService.GetDataAsync();
        }
    }
}
