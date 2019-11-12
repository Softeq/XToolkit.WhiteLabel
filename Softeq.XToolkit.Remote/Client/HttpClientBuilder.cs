using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly Lazy<HttpHandlerBuilder> _builderOfMessageHandlerLazy;

        public HttpClientBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
            _builderOfMessageHandlerLazy = new Lazy<HttpHandlerBuilder>(() => new HttpHandlerBuilder());
        }

        // TODO YP: handlers order
        //     Fix ability for public change order: diagnostic) AuthHandler
        // Correct order:
        //     NativeHandler) PriorityHandler) AuthHandler) DiagnosticHandler
        public IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler)
        {
            _builderOfMessageHandlerLazy.Value.AddHandler(delegatingHandler);
            return this;
        }

        public virtual HttpClient Build()
        {
            var handler = GetHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            httpClient.BaseAddress = new Uri(_baseUrl);

            return httpClient;
        }

        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            return _builderOfMessageHandlerLazy.Value.Build();
        }

        protected virtual HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }
    }
}
