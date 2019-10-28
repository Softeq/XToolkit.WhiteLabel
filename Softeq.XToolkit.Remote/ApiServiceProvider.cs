using System;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public interface IApiServiceProvider<out TApiService>
    {
        TApiService Get();

        TApiService GetByPriority(RequestPriority requestPriority);
    }

    public class ApiServiceProvider<TApiService> : IApiServiceProvider<TApiService>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiServiceFactory _apiServiceFactory;

        private readonly Lazy<TApiService> _userInitiated;
        private readonly Lazy<TApiService> _background;
        private readonly Lazy<TApiService> _speculative;

        public ApiServiceProvider(
            IHttpClientFactory httpClientFactory,
            IApiServiceFactory apiServiceFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiServiceFactory = apiServiceFactory;

            _userInitiated = new Lazy<TApiService>(() => CreateService(RequestPriority.UserInitiated));
            _background = new Lazy<TApiService>(() => CreateService(RequestPriority.Background));
            _speculative = new Lazy<TApiService>(() => CreateService(RequestPriority.Speculative));
        }

        public TApiService Get()
        {
            return _speculative.Value;
        }

        public TApiService GetByPriority(RequestPriority requestPriority)
        {
            switch (requestPriority)
            {
                case RequestPriority.UserInitiated:
                    return _userInitiated.Value;
                case RequestPriority.Background:
                    return _background.Value;
                default:
                    return _speculative.Value;
            }
        }

        protected virtual TApiService CreateService(RequestPriority requestPriority)
        {
            var httpClient = _httpClientFactory.CreateWithPriority(requestPriority);
            return _apiServiceFactory.Create<TApiService>(httpClient);
        }
    }
}
