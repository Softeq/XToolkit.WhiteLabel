// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> RequestPermissionsAsync<T>() where T : BasePermission, new();
        Task<PermissionStatus> CheckPermissionsAsync<T>() where T : BasePermission, new();
        void OpenSettings();
    }
}
