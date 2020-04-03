// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using UserNotifications;

#pragma warning disable SA1300
namespace Softeq.XToolkit.PushNotifications.iOS
#pragma warning restore SA1300
{
    public class IosNotificationsPermissionsService : INotificationsPermissionsService
    {
        /// <summary>
        ///     Gets UNAuthorizationOptions for requesting notifications permissions.
        ///     Default value is Alert and Sound. Override to provide custom values.
        /// </summary>
        protected virtual UNAuthorizationOptions RequiredAuthOptions => UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound;

        public async Task<bool> RequestNotificationsPermissions()
        {
            var (isGranted, _) = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(RequiredAuthOptions);
            return isGranted;
        }
    }
}
