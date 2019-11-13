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
            var handler = new RefreshTokenHttpClientHandler(
                () => Task.FromResult(sessionContext.AccessToken),
                sessionContext.RefreshTokenAsync);

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
