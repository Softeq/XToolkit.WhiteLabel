using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder AddHandler(DelegatingHandler delegatingHandler);
        HttpClient Build();
    }
}
