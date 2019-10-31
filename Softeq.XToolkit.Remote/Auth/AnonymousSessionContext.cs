using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    public class AnonymousSessionContext : ISessionContext
    {
        public string AccessToken => string.Empty;

        public Task RefreshTokenAsync() => Task.CompletedTask;
    }
}
