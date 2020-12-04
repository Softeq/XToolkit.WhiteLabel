// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth;

namespace Playground.RemoteData.GitHub
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
