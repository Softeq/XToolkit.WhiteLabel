// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    public interface ISessionContext
    {
        string AccessToken { get; }

        Task RefreshTokenAsync();
    }
}
