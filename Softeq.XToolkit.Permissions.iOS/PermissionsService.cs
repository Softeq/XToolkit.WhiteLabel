// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Plugin.Permissions;
using UIKit;
using UserNotifications;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS
{
    /// <inheritdoc cref="IPermissionsService" />
    public class PermissionsService : IPermissionsService
    {
        /// <inheritdoc />
        public async Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            if (typeof(T) == typeof(NotificationsPermission))
            {
                return await RequestNotificationPermissionAsync().ConfigureAwait(false);
            }

            var result = await CrossPermissions.Current.RequestPermissionAsync<T>().ConfigureAwait(false);

            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            if (typeof(T) == typeof(NotificationsPermission))
            {
                return await CheckNotificationsPermissionAsync().ConfigureAwait(false);
            }

            var result = await CrossPermissions.Current.CheckPermissionStatusAsync<T>().ConfigureAwait(false);

            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public void OpenSettings()
        {
            RunInMainThread(() => { CrossPermissions.Current.OpenAppSettings(); });
        }

        // TODO YP: use common dependency
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

        // TODO YP: refactor to using partial NotificationsPermission for iOS.
        private static async Task<PermissionStatus> CheckNotificationsPermissionAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var notificationSettings = await notificationCenter.GetNotificationSettingsAsync().ConfigureAwait(false);
            if (notificationSettings.AuthorizationStatus == UNAuthorizationStatus.NotDetermined)
            {
                return PermissionStatus.Unknown;
            }

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
    }
}
