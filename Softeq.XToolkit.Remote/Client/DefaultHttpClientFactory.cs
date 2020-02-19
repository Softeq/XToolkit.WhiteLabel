// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public virtual HttpClient CreateClient(string baseUrl, ILogger logger = null)
        {
            var builder = new HttpClientBuilder(baseUrl);

            AddLoggerIfNeeded(builder, logger);

            return builder.Build();
        }

        public virtual HttpClient CreateAuthClient(string baseUrl, ISessionContext sessionContext, ILogger logger = null)
        {
            var builder = new HttpClientBuilder(baseUrl);

            AddLoggerIfNeeded(builder, logger);

            builder.WithSessionContext(sessionContext);

            return builder.Build();
        }

        protected static void AddLoggerIfNeeded(IHttpClientBuilder builder, ILogger logger)
        {
            if (logger != null)
            {
                builder.WithLogger(logger);
            }
        }
    }
}
