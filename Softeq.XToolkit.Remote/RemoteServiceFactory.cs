using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteServiceFactory
    {
        IRemoteService<T> Create<T>(HttpClient httpClient);
    }

    public class RemoteServiceFactory : IRemoteServiceFactory
    {
        public IRemoteService<T> Create<T>(HttpClient httpClient)
        {
            var refitService = new ApiService<T>(httpClient);

            return new RemoteService<T>(refitService);
        }


    }
}
