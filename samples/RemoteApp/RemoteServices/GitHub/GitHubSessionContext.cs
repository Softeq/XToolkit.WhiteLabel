using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth;

namespace RemoteServices.GitHub
{
    public class GitHubSessionContext : ISessionContext
    {
        public GitHubSessionContext(string token)
        {
            AccessToken = token;
        }

        public string AccessToken { get; }

        public Task RefreshTokenAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
