// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionsManager
    {
        Task<PermissionStatus> CheckWithRequestAsync<T>() where T : BasePermission, new();
        Task<PermissionStatus> CheckAsync<T>() where T : BasePermission, new();
        void SetPermissionDialogService(IPermissionsDialogService permissionsDialogService);
    }
}
