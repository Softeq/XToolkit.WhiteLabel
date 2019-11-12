using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Client.Handlers;

namespace Softeq.XToolkit.Remote.Auth
{
    public static class Extensions
    {
        public static IHttpClientBuilder WithSessionContext(
            this IHttpClientBuilder httpClientBuilder,
            ISessionContext sessionContext)
        {
            Func<Task<string>> accessToken = () => Task.FromResult(sessionContext.AccessToken);

//            var handler = new AuthenticatedHttpClientHandler(accessToken);

            var handler = new RefreshTokenHttpClientHandler(accessToken,
                async () =>
                {
                    await sessionContext.RefreshTokenAsync().ConfigureAwait(false);
                    return sessionContext.AccessToken;
                });

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
