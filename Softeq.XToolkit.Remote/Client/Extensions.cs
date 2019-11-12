using System.ComponentModel;
using System.Net.Http;
using Fusillade;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client
{
    public static class Extensions
    {
        public static IHttpClientBuilder WithLogger(
            this IHttpClientBuilder httpClientBuilder,
            ILogger logger)
        {
            var handler = new HttpDiagnosticsHandler(logger);
            return httpClientBuilder.AddHandler(handler);
        }

        internal static IHttpClientBuilder WithPriority(
            this IHttpClientBuilder httpClientBuilder,
            RequestPriority requestPriority)
        {
            DelegatingHandler innerHandler;

            switch (requestPriority)
            {
                case RequestPriority.Speculative:
                    innerHandler = NetCache.Speculative;
                    break;
                case RequestPriority.UserInitiated:
                    innerHandler = (RateLimitedHttpMessageHandler)NetCache.UserInitiated;
                    break;
                case RequestPriority.Background:
                    innerHandler = (RateLimitedHttpMessageHandler)NetCache.Background;
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(requestPriority), (int) requestPriority, typeof(Priority));
            }

            return httpClientBuilder.AddHandler(innerHandler);
        }
    }
}
