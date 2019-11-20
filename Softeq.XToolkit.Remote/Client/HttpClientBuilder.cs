using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <inheritdoc />
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly HttpMessageHandlerBuilder _httpMessageHandlerBuilder;

        public HttpClientBuilder(string baseUrl) : this(baseUrl, new DefaultHttpMessageHandlerBuilder())
        {
        }

        public HttpClientBuilder(string baseUrl, HttpMessageHandlerBuilder httpMessageHandlerBuilder)
        {
            _baseUrl = baseUrl;
            _httpMessageHandlerBuilder = httpMessageHandlerBuilder;
        }

        /// <inheritdoc />
        public IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler)
        {
            _httpMessageHandlerBuilder.AddHandler(delegatingHandler);
            return this;
        }

        /// <inheritdoc />
        public virtual HttpClient Build()
        {
            var handler = GetHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            httpClient.BaseAddress = new Uri(_baseUrl);

            return httpClient;
        }

        protected virtual HttpMessageHandler GetHttpMessageHandler()
        {
            return _httpMessageHandlerBuilder.Build();
        }

        protected virtual HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }
    }
}
