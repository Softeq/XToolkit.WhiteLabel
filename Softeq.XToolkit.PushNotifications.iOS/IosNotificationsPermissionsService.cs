// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosNotificationsPermissionsService : INotificationsPermissionsService
    {
        /// <summary>
        /// UNAuthorizationOptions for requesting notifications permissions.
        /// Default value is Alert and Sound. Override to provide custom values.
        /// </summary>
        protected virtual UNAuthorizationOptions RequiredAuthOptions => UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound;

        public async Task<bool> RequestNotificationsPermissions()
        {
            var (isGranted, _) =  await UNUserNotificationCenter.Current.RequestAuthorizationAsync(RequiredAuthOptions);
            return isGranted;
        }
    }
}
