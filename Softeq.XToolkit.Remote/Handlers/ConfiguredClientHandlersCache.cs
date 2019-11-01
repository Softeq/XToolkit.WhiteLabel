using System.ComponentModel;
using System.Net.Http;
using Fusillade;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Handlers
{
    internal static class ConfiguredClientHandlersCache
    {
        internal static DelegatingHandler GetByPriority(HttpMessageHandler innerHandler, RequestPriority requestPriority)
        {
            // TODO YP: Based on Fusillade default handlers:
            // https://github.com/reactiveui/Fusillade/blob/master/src/Fusillade/NetCache.cs#L47-L50
            // need to resolve problem with share native message handler.
            // Locator.CurrentMutable.RegisterConstant(new NativeMessageHandler(), typeof(HttpMessageHandler));
            switch (requestPriority)
            {
                case RequestPriority.Speculative:
                    const int maxBytesToRead = 1048576 * 5;
                    return new RateLimitedHttpMessageHandler(innerHandler, Priority.Speculative, 0, maxBytesToRead);
                case RequestPriority.UserInitiated:
                    return new RateLimitedHttpMessageHandler(innerHandler, Priority.UserInitiated);
                case RequestPriority.Background:
                    return new RateLimitedHttpMessageHandler(innerHandler, Priority.Background);
                default:
                    throw new InvalidEnumArgumentException(nameof(requestPriority), (int) requestPriority, typeof(Priority));
            }
        }
    }
}
