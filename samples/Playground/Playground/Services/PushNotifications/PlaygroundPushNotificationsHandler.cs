// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Playground.Services.PushNotifications
{
    public class PlaygroundPushNotificationsHandler : IPushNotificationsHandler
    {
        public void OnPushRegistrationCompleted(bool isRegisteredInSystem, bool isSavedOnServer)
        {
        }

        public void OnPushPermissionsRequestCompleted(bool permissionsGranted)
        {
        }

        public void HandlePushNotificationReceived(PushNotificationModel pushNotification, bool inForeground)
        {
        }

        public void HandlePushNotificationTapped(PushNotificationModel pushNotification)
        {
        }

        public void HandleSilentPushNotification(PushNotificationModel pushNotification)
        {
        }

        public void HandleInvalidPushNotification(Exception exception, object pushNotification)
        {
        }
    }
}
