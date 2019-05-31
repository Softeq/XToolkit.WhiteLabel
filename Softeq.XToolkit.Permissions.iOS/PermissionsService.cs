// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.ComponentModel;
using Foundation;
using UIKit;
using UserNotifications;
using Plugin.Permissions;
using PluginPermission = Plugin.Permissions.Abstractions.Permission;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> RequestPermissionsAsync(Permission permission)
        {
            if (permission == Permission.Notifications)
            {
                return await RequestNotificationPermissionAsync().ConfigureAwait(false);
            }

            var pluginPermission = ToPluginPermission(permission);
            var result = await CrossPermissions.Current.RequestPermissionsAsync(pluginPermission).ConfigureAwait(false);
            
            return result.TryGetValue(pluginPermission, out var permissionStatus)
                ? ToPermissionStatus(permissionStatus)
                : PermissionStatus.Unknown;
        }

        public async Task<PermissionStatus> CheckPermissionsAsync(Permission permission)
        {
            if (permission == Permission.Notifications)
            {
                return await CheckNotificationsPermissionAsync().ConfigureAwait(false);
            }

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
            if (NSThread.IsMain)
            {
                action();
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(action);
            }
        }

        private static async Task<PermissionStatus> CheckNotificationsPermissionAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var notificationSettings = await notificationCenter.GetNotificationSettingsAsync().ConfigureAwait(false);
            var notificationsSettingsEnabled = notificationSettings.SoundSetting == UNNotificationSetting.Enabled
                && notificationSettings.AlertSetting == UNNotificationSetting.Enabled;
            return notificationsSettingsEnabled
                ? PermissionStatus.Granted
                : PermissionStatus.Denied;
        }

        private static async Task<PermissionStatus> RequestNotificationPermissionAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var (isGranted, _) = await notificationCenter.RequestAuthorizationAsync(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound);
            return isGranted
                ? PermissionStatus.Granted
                : PermissionStatus.Denied;
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
                case Permission.Storage:
                    return PluginPermission.Storage;
                case Permission.Photos:
                    return PluginPermission.Photos;
                case Permission.LocationInUse:
                    return PluginPermission.LocationWhenInUse;
                default:
                    throw new NotImplementedException(
                        $"Permissions does not work with {permission} permissions. Please handle it separately");
            }
        }
    }
}