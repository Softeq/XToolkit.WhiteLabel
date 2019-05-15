// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Plugin.Permissions;
using UIKit;
using UserNotifications;
using System.ComponentModel;

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
            var result = await CrossPermissions.Current.RequestPermissionsAsync(pluginPermission);
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

            var result = await CrossPermissions.Current
                .CheckPermissionStatusAsync(ToPluginPermission(permission)).ConfigureAwait(false);
            return ToPermissionStatus(result);
        }

        public void OpenSettings()
        {
            if (NSThread.IsMain)
            {
                CrossPermissions.Current.OpenAppSettings();
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    CrossPermissions.Current.OpenAppSettings();
                });
            }
        }

        private async Task<PermissionStatus> CheckNotificationsPermissionAsync()
        {
            var notificationSettings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync().ConfigureAwait(false);
            var notificationsSettingsEnabled = notificationSettings.SoundSetting == UNNotificationSetting.Enabled
                && notificationSettings.AlertSetting == UNNotificationSetting.Enabled;
            return notificationsSettingsEnabled
                ? PermissionStatus.Granted : PermissionStatus.Denied;
        }

        private static async Task<PermissionStatus> RequestNotificationPermissionAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var result = await notificationCenter.RequestAuthorizationAsync(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound);
            return result.Item1 ? PermissionStatus.Granted : PermissionStatus.Denied;
        }

        private PermissionStatus ToPermissionStatus(Plugin.Permissions.Abstractions.PermissionStatus permissionStatus)
        {
            switch (permissionStatus)
            {
                case Plugin.Permissions.Abstractions.PermissionStatus.Denied:
                    return PermissionStatus.Denied;
                case Plugin.Permissions.Abstractions.PermissionStatus.Disabled:
                    return PermissionStatus.Denied;
                case Plugin.Permissions.Abstractions.PermissionStatus.Granted:
                    return PermissionStatus.Granted;
                case Plugin.Permissions.Abstractions.PermissionStatus.Restricted:
                    return PermissionStatus.Denied;
                case Plugin.Permissions.Abstractions.PermissionStatus.Unknown:
                    return PermissionStatus.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(nameof(permissionStatus), permissionStatus, null);
            }
        }

        private static Plugin.Permissions.Abstractions.Permission ToPluginPermission(Permission permission)
        {
            switch (permission)
            {
                case Permission.Camera:
                    return Plugin.Permissions.Abstractions.Permission.Camera;
                case Permission.Storage:
                    return Plugin.Permissions.Abstractions.Permission.Storage;
                case Permission.Photos:
                    return Plugin.Permissions.Abstractions.Permission.Photos;
                case Permission.LocationInUse:
                    return Plugin.Permissions.Abstractions.Permission.LocationWhenInUse;
                case Permission.Notifications:
                    throw new InvalidEnumArgumentException(
                        $"Plugin.Permissions does not work with {permission} permissions. " +
                        "Please handle it separately");
                default:
                    throw new NotImplementedException();
            }
        }
    }
}