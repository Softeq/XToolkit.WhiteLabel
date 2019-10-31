using System;
using System.Net.Http;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder WithLogger(ILogger logger);
        IHttpClientBuilder WithCustomHandler(Func<HttpMessageHandler, HttpMessageHandler> messageHandler); // TODO YP: remove & add builder
        HttpClient Build();
    }
}
