using System.Net.Http;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientFactory
    {
        HttpClient CreateWithPriority(RequestPriority requestPriority);
    }
}