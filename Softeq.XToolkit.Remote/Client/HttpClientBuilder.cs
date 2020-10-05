// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <inheritdoc cref="IHttpClientBuilder"/>
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly HttpMessageHandlerBuilder _httpMessageHandlerBuilder;

        public HttpClientBuilder(string baseUrl)
            : this(baseUrl, new DefaultHttpMessageHandlerBuilder())
        {
        }

        public HttpClientBuilder(string baseUrl, HttpMessageHandlerBuilder httpMessageHandlerBuilder)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Can't be null or empty", nameof(baseUrl));
            }

            _baseUrl = baseUrl;
            _httpMessageHandlerBuilder = httpMessageHandlerBuilder ?? throw new ArgumentNullException(nameof(httpMessageHandlerBuilder));
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
            var handler = CreateHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            httpClient.BaseAddress = new Uri(_baseUrl);

            return httpClient;
        }

        protected virtual HttpMessageHandler CreateHttpMessageHandler()
        {
            return _httpMessageHandlerBuilder.Build();
        }

        protected virtual HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }
    }
}
