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

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpClientBuilder"/> class.
        /// </summary>
        /// <param name="baseUrl">The base address is used in HttpClient.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="baseUrl"/> parameter is null or is an empty string.
        /// </exception>
        public HttpClientBuilder(string baseUrl)
            : this(baseUrl, new DefaultHttpMessageHandlerBuilder())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpClientBuilder"/> class with custom HttpMessageHandler builder.
        /// </summary>
        /// <param name="baseUrl">The base address is used in HttpClient.</param>
        /// <param name="httpMessageHandlerBuilder">Builder to create a chain of HttpMessageHandler.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="baseUrl"/> parameter is null or is an empty string.
        /// </exception>
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
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="delegatingHandler"/> parameter is null.
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
