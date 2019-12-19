using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth
{
    public interface ISessionContext
    {
        string AccessToken { get; }

        Task RefreshTokenAsync();
    }
}
