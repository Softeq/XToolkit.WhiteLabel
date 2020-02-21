// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;

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
    }
}
