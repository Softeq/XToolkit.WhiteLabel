using System;
using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Handlers;

namespace Softeq.XToolkit.Remote.Client
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        protected readonly string BaseUrl;

        private ILogger _logger;
        private readonly Lazy<HttpHandlerBuilder> _clientHandlersBuilder;

        public HttpClientBuilder(string baseUrl)
        {
            BaseUrl = baseUrl;

            _clientHandlersBuilder = new Lazy<HttpHandlerBuilder>(() => new HttpHandlerBuilder());
        }

        public IHttpClientBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IHttpClientBuilder AddHandler(Func<HttpMessageHandler, DelegatingHandler> delegatingHandler)
        {
            _clientHandlersBuilder.Value.AddHandler(delegatingHandler);
            return this;
        }

        public virtual HttpClient Build()
        {
            var handler = GetHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            httpClient.BaseAddress = new Uri(BaseUrl);

            return httpClient;
        }

        // NativeHandler) PriorityHandler) AuthHandler) DiagnosticHandler
        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            var handler = _clientHandlersBuilder.Value.Build();

#if DEBUG
            handler = new HttpDiagnosticsHandler(handler, _logger);
#endif

            return handler;
        }

        protected virtual HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }
    }
}
