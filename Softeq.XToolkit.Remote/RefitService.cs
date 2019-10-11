using System;
using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote
{
    public interface IRefitService<T>
    {
        T GetByPriority(Priority priority);
    }


    public class RefitService<T> : IRefitService<T>
    {
        private readonly Lazy<T> _coreService;

        public RefitService(HttpClient httpClient)
        {
            Func<HttpMessageHandler, T> createClient = messageHandler =>
            {
                return RestService.For<T>(httpClient);
            };

            _coreService = new Lazy<T>(() => createClient(null));

            //_background = new Lazy<T>(() => createClient(NetCache.Background));
        }

        protected virtual void DisableAutoRedirects(HttpMessageHandler messageHandler)
        {
            if (messageHandler is DelegatingHandler internalDelegate
                && internalDelegate.InnerHandler is HttpClientHandler internalClientHandler)
            {
                internalClientHandler.AllowAutoRedirect = false;
            }
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
