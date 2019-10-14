using System;
using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote
{
    public interface IApiService<T>
    {
        T GetByPriority(Priority priority);
    }


    public class ApiService<T> : IApiService<T>
    {
        private readonly Lazy<T> _coreService;

        public ApiService(HttpClient httpClient)
        {
            Func<HttpMessageHandler, T> createClient = messageHandler =>
            {
                return RestService.For<T>(httpClient);
            };

            _coreService = new Lazy<T>(() => createClient(null));

            //_background = new Lazy<T>(() => createClient(NetCache.Background));
        }

        public T GetByPriority(Priority priority)
        {
            //switch (priority)
            //{
            //    case Priority.Background:
            //        return _background.Value;
            //    case Priority.Speculative:
            //        return _speculative.Value;
            //    case Priority.UserInitiated:
            //    default:
            //        return _userInitiated.Value;
            //}

            return _coreService.Value;
        }
    }
}
