// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionsManager
    {
        Task<PermissionStatus> CheckWithRequestAsync(Permission permission);
        Task<PermissionStatus> CheckAsync(Permission permission);
    }
}
