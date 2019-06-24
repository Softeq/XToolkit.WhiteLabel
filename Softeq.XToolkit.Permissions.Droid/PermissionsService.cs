// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.OS;
using Plugin.Permissions;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await CrossPermissions.Current.RequestPermissionAsync<T>().ConfigureAwait(false);
            return ToPermissionStatus(result);
        }

        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await CrossPermissions.Current.CheckPermissionStatusAsync<T>().ConfigureAwait(false);
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
                        (int) permissionStatus, permissionStatus.GetType());
            }
        }
    }
}
