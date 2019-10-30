using System;
using System.Net.Http;
using System.Reflection;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Handlers;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder WithLogger(ILogger logger);
        IHttpClientBuilder WithCustomHandler(Func<HttpMessageHandler, HttpMessageHandler> messageHandler);
        HttpClient Build();
    }

    public class HttpClientBuilder : IHttpClientBuilder
    {
        protected readonly string BaseUrl;

        private ILogger _logger;
        private Func<HttpMessageHandler, HttpMessageHandler> _customHandler;

        public HttpClientBuilder(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public IHttpClientBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IHttpClientBuilder WithCustomHandler(Func<HttpMessageHandler, HttpMessageHandler> customHandler)
        {
            _customHandler = customHandler;
            return this;
        }

        public IHttpClientBuilder WithoutAutoRedirects()
        {
            //if (messageHandler is DelegatingHandler internalDelegate
            //    && internalDelegate.InnerHandler is HttpClientHandler internalClientHandler)
            //{
            //    internalClientHandler.AllowAutoRedirect = false;
            //}
            return this;
        }

        public virtual HttpClient Build()
        {
            var handler = GetHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            httpClient.BaseAddress = new Uri(BaseUrl);

            return httpClient;
        }

        // NativeHandler <- PriorityHandler (<- AuthHandler) <- DiagnosticHandler
        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            var handler = CreateDefaultHandler();

            if (_customHandler != null)
            {
                handler = _customHandler(handler);
            }

#if DEBUG
            handler = new HttpDiagnosticsHandler(handler, _logger);
#endif

            return handler;
        }

        protected virtual HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }

        protected virtual HttpMessageHandler CreateDefaultHandler()
        {
            // HACK YP: need check, because linker can change assembly.
            // Sources: https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/HttpClient.DefaultHandler.cs#L5

            var method = typeof(HttpClient).GetMethod("CreateDefaultHandler", BindingFlags.NonPublic | BindingFlags.Static);
            return method?.Invoke(null, null) as HttpMessageHandler;
        }
    }
}
