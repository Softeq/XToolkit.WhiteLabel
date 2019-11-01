using System;
using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder WithLogger(ILogger logger);
        IHttpClientBuilder AddHandler(Func<HttpMessageHandler, DelegatingHandler> delegatingHandler);
        HttpClient Build();
    }
}
