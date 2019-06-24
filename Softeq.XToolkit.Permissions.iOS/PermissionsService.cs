// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.ComponentModel;
using Foundation;
using UIKit;
using UserNotifications;
using Plugin.Permissions;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            if (typeof(T) == typeof(NotificationsPermission))
            {
                return await RequestNotificationPermissionAsync().ConfigureAwait(false);
            }

            var result = await CrossPermissions.Current.RequestPermissionAsync<T>().ConfigureAwait(false);

            return ToPermissionStatus(result);
        }

        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            if (typeof(T) == typeof(NotificationsPermission))
            {
                return await CheckNotificationsPermissionAsync().ConfigureAwait(false);
            }

            var result = await CrossPermissions.Current.CheckPermissionStatusAsync<T>().ConfigureAwait(false);
            
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
    }
}