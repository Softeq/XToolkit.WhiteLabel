// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.OS;
using Plugin.Permissions;
using PluginPermission = Plugin.Permissions.Abstractions.Permission;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> RequestPermissionsAsync(Permission permission)
        {
            var pluginPermission = ToPluginPermission(permission);
            var result = await CrossPermissions.Current.RequestPermissionsAsync(pluginPermission).ConfigureAwait(false);
            
            return result.TryGetValue(pluginPermission, out var permissionStatus) 
                ? ToPermissionStatus(permissionStatus) 
                : PermissionStatus.Unknown;
        }

        public async Task<PermissionStatus> CheckPermissionsAsync(Permission permission)
        {
            var pluginPermission = ToPluginPermission(permission);
            var result = await CrossPermissions.Current.CheckPermissionStatusAsync(pluginPermission).ConfigureAwait(false);
            
            return ToPermissionStatus(result);
        }

        public void OpenSettings()
        {
            RunInMainThread(() => { CrossPermissions.Current.OpenAppSettings(); });
        }

        private static void RunInMainThread(Action action)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                action();
            }
            else
            {
                var _ = new Handler(Looper.MainLooper).Post(action);
            }
        }

        private static PermissionStatus ToPermissionStatus(PluginPermissionStatus permissionStatus)
        {
            switch (permissionStatus)
            {
                case PluginPermissionStatus.Denied:
                    return PermissionStatus.Denied;
                case PluginPermissionStatus.Disabled:
                    return PermissionStatus.Denied;
                case PluginPermissionStatus.Granted:
                    return PermissionStatus.Granted;
                case PluginPermissionStatus.Restricted:
                    return PermissionStatus.Denied;
                case PluginPermissionStatus.Unknown:
                    return PermissionStatus.Unknown;
                default:
                    throw new InvalidEnumArgumentException(nameof(permissionStatus),
                        (int)permissionStatus, permissionStatus.GetType());
            }
        }

        private static PluginPermission ToPluginPermission(Permission permission)
        {
            switch (permission)
            {
                case Permission.Camera:
                    return PluginPermission.Camera;
                case Permission.Photos:
                    return PluginPermission.Storage;
                case Permission.LocationInUse:
                    return PluginPermission.Location;
                default:
                    throw new NotImplementedException(
                        $"Permissions does not work with {permission} permissions. Please handle it separately");
            }
        }
        
        private static Permission ToPermission(PluginPermission permission)
        {
            switch (permission)
            {
                case PluginPermission.Camera:
                    return Permission.Camera;
                case PluginPermission.Storage:
                    return Permission.Photos;
                case PluginPermission.Location:
                    return Permission.LocationInUse;
                default:
                    throw new NotImplementedException(
                        $"Permissions does not work with {permission} permissions. Please handle it separately");
            }
        }
    }
}
