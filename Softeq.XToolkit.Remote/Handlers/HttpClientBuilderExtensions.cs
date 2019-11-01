using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Handlers
{
    internal static class HttpClientBuilderExtensions
    {
        internal static IHttpClientBuilder WithPriority(this IHttpClientBuilder httpClientBuilder, RequestPriority requestPriority)
        {
            return httpClientBuilder.AddHandler(innerHandler =>
                ConfiguredClientHandlersCache.GetByPriority(innerHandler, requestPriority));
        }
    }
}
