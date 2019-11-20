using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    // YP: simple factoy for create HttpClient
    // Responsibility:
    // - return configured HttpClient
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient(string baseUrl);
    }

    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateHttpClient(string baseUrl)
        {
            var httpHandler = HttpHandlerBuilder.NativeHandler;

            return new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}
