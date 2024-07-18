// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using UserNotifications;
using BasePlatformPermission = Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission;
using EssentialsPermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS.Permissions
{
    public class Notifications : BasePlatformPermission
    {
        public static UNAuthorizationOptions AuthorizationOptions =
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound;

        /// <inheritdoc />
        public override async Task<EssentialsPermissionStatus> CheckStatusAsync()
        {
            var notificationSettings = await UNUserNotificationCenter.Current
                .GetNotificationSettingsAsync().ConfigureAwait(false);

            return ParseAuthorization(notificationSettings.AuthorizationStatus);
        }

        /// <inheritdoc />
        public override async Task<EssentialsPermissionStatus> RequestAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var (isGranted, _) = await notificationCenter
                .RequestAuthorizationAsync(AuthorizationOptions)
                .ConfigureAwait(false);
            return isGranted
                ? EssentialsPermissionStatus.Granted
                : EssentialsPermissionStatus.Denied;
        }

        private static EssentialsPermissionStatus ParseAuthorization(UNAuthorizationStatus authorizationStatus)
        {
            return authorizationStatus switch
            {
                UNAuthorizationStatus.NotDetermined => EssentialsPermissionStatus.Unknown,
                UNAuthorizationStatus.Authorized => EssentialsPermissionStatus.Granted,
                UNAuthorizationStatus.Provisional => EssentialsPermissionStatus.Granted,
                UNAuthorizationStatus.Denied => EssentialsPermissionStatus.Denied,
                _ => EssentialsPermissionStatus.Unknown
            };
        }
    }
}
