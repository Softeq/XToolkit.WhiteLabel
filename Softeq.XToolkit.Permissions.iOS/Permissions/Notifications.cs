// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBluetooth;
using UserNotifications;
using BasePlatformPermission = Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission;
using EssentialsPermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS.Permissions
{
    public class Notifications : BasePlatformPermission
    {
        public override async Task<EssentialsPermissionStatus> CheckStatusAsync()
        {
            var notificationSettings = await UNUserNotificationCenter.Current
                .GetNotificationSettingsAsync().ConfigureAwait(false);

            return ParseAuthorization(notificationSettings.AuthorizationStatus);
        }

        public override async Task<EssentialsPermissionStatus> RequestAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var (isGranted, _) = await notificationCenter.RequestAuthorizationAsync(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound);
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