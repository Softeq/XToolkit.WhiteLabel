using System.Net.Http;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(HttpClient httpClient);
    }

    public class RestRemoteServiceFactory : IRemoteServiceFactory
    {
        public IRemoteService<T> Create<T>(HttpClient httpClient)
        {
            var refitService = new RefitService<T>(httpClient);

            return new RestRemoteService<T>(refitService);
        }
    }
}
