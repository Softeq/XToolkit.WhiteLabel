// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <inheritdoc cref="IHttpClientBuilder"/>
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly HttpMessageHandlerBuilder _httpMessageHandlerBuilder;

        private string? _baseUrl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpClientBuilder"/> class.
        /// </summary>
        public HttpClientBuilder()
            : this(new DefaultHttpMessageHandlerBuilder())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpClientBuilder"/> class with custom HttpMessageHandler builder.
        /// </summary>
        /// <param name="httpMessageHandlerBuilder">Builder to create a chain of HttpMessageHandler.</param>
        public HttpClientBuilder(HttpMessageHandlerBuilder httpMessageHandlerBuilder)
        {
            _httpMessageHandlerBuilder = httpMessageHandlerBuilder ?? throw new ArgumentNullException(nameof(httpMessageHandlerBuilder));
        }

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="delegatingHandler"/> parameter is <see langword="null"/>.
        /// </exception>
        public IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler)
        {
            if (delegatingHandler == null)
            {
                throw new ArgumentNullException(nameof(delegatingHandler));
            }

            _httpMessageHandlerBuilder.AddHandler(delegatingHandler);

            return this;
        }

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="baseUrl"/> parameter is <see langword="null"/> or an empty string.
        /// </exception>
        public IHttpClientBuilder WithBaseUrl(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Can't be null or empty", nameof(baseUrl));
            }

            _baseUrl = baseUrl;

            return this;
        }

        /// <inheritdoc />
        public virtual HttpClient Build()
        {
            var handler = CreateHttpMessageHandler();
            var httpClient = CreateHttpClient(handler);

            if (!string.IsNullOrEmpty(_baseUrl))
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
            }

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
