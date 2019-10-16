using System;
using System.ComponentModel;
using System.Net.Http;
using Fusillade;
using Refit;
using Softeq.XToolkit.Remote.Client;
using InternalPriority = Softeq.XToolkit.Remote.Primitives.Priority;

namespace Softeq.XToolkit.Remote
{
    public interface IApiServiceProvider<out TApiService>
    {
        TApiService GetByPriority(InternalPriority priority);
    }

    public class ApiServiceProvider<TApiService> : IApiServiceProvider<TApiService>
    {
        private readonly Lazy<TApiService> _userInitiated;
        private readonly Lazy<TApiService> _background;
        private readonly Lazy<TApiService> _speculative;

        public ApiServiceProvider(IHttpClientBuilder httpClientBuilder)
        {
            _userInitiated = new Lazy<TApiService>(() => CreateService(httpClientBuilder, Priority.UserInitiated));
            _background = new Lazy<TApiService>(() => CreateService(httpClientBuilder, Priority.Background));
            _speculative = new Lazy<TApiService>(() => CreateService(httpClientBuilder, Priority.Speculative));
        }

        public TApiService GetByPriority(InternalPriority priority)
        {
            switch (priority)
            {
                case InternalPriority.UserInitiated:
                    return _userInitiated.Value;
                case InternalPriority.Background:
                    return _background.Value;
                default:
                    return _speculative.Value;
            }
        }

        protected virtual TApiService CreateService(IHttpClientBuilder httpClientBuilder, Priority priority)
        {
            var httpClient = httpClientBuilder
                    .WithCustomHandler(innerHandler => GetHandlerByPriority(innerHandler, priority))
                    .Build();

            return RestService.For<TApiService>(httpClient);
        }

        protected virtual HttpMessageHandler GetHandlerByPriority(HttpMessageHandler innerHandler, Priority priority)
        {
            // Based on Fusillade default handlers:
            // https://github.com/reactiveui/Fusillade/blob/master/src/Fusillade/NetCache.cs#L47-L50
            switch (priority)
            {
                case Priority.Speculative:
                    const int maxBytesToRead = 1048576 * 5;
                    return new RateLimitedHttpMessageHandler(innerHandler, Priority.Speculative, 0, maxBytesToRead);
                case Priority.UserInitiated:
                case Priority.Background:
                    return new RateLimitedHttpMessageHandler(innerHandler, priority);
                default:
                    throw new InvalidEnumArgumentException(nameof(priority), (int) priority, typeof(Priority));
            }
        }
    }
}
