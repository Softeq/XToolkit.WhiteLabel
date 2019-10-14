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
        IHttpClientBuilder WithDelegatingHandler(Func<HttpMessageHandler, HttpMessageHandler> messageHandler);
        HttpClient Build();
    }

    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly string _baseUrl;

        private ILogger _logger;
        private Func<HttpMessageHandler, HttpMessageHandler> _messageHandler;

        public HttpClientBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public IHttpClientBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IHttpClientBuilder WithDelegatingHandler(Func<HttpMessageHandler, HttpMessageHandler> messageHandler)
        {
            _messageHandler = messageHandler;
            return this;
        }

        //public IHttpClientBuilder WithAuthentication(string headerName)
        //{
        //    return this;
        //}

        public HttpClient Build()
        {
            var httpClient = CreateHttpClient();

            httpClient.BaseAddress = new Uri(_baseUrl);

            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return httpClient;
        }

        protected virtual HttpClient CreateHttpClient()
        {
            var handler = GetHttpMessageHandler();

            if (_messageHandler != null)
            {
                handler = _messageHandler(handler);
            }

#if DEBUG
            return new HttpClient(new HttpDiagnosticsHandler(handler, _logger));
#else
            return new HttpClient(handler);
#endif
        }

        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            return CreateDefaultHandler();
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
