using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Handlers;

namespace Softeq.XToolkit.Remote
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder WithLogger(ILogger logger);
        HttpClient Build();
    }

    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly string _baseUrl;

        private ILogger _logger;

        public HttpClientBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public IHttpClientBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IHttpClientBuilder WithAuthentication(string headerName)
        {
            return this;
        }

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
            var handler = GetHttpMessageHander();
#if DEBUG
            return new HttpClient(new HttpDiagnosticsHandler(handler, _logger));
#else
            return new HttpClient(handler);
#endif
        }

        protected virtual HttpMessageHandler GetHttpMessageHander()
        {
            return CreateDefaultHandler();
        }

        protected virtual HttpMessageHandler CreateDefaultHandler()
        {
            // HACK YP: need check, because linker can change assembly.
            // Sources: https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/HttpClient.DefaultHandler.cs#L5

            var method = typeof(HttpClient).GetMethod("CreateDefaultHandler", BindingFlags.NonPublic | BindingFlags.Static);
            return method.Invoke(null, null) as HttpMessageHandler;
        }
    }
}
