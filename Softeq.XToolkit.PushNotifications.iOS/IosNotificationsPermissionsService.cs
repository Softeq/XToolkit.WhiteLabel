using System;
using System.Threading.Tasks;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosNotificationsPermissionsService : INotificationsPermissionsService
    {
        /// <summary>
        /// UNAuthorizationOptions for requesting notifications permissions. Default value is Alert and Sound. Override to provide custom values
        /// </summary>
        public virtual UNAuthorizationOptions RequiredAuthOptions => UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound;

        public async Task<bool> RequestNotificationsPermissions()
        {
            var permissionStatus = await RequestNotificationsPermissionAsync().ConfigureAwait(false);
            return permissionStatus;
        }

        private async Task<bool> RequestNotificationsPermissionAsync()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var (isGranted, _) = await notificationCenter.RequestAuthorizationAsync(RequiredAuthOptions);
            return isGranted;
        }
    }
}