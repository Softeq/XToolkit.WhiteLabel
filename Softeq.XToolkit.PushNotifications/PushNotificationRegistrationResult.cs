// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    ///     Class containing push notifications registration result
    ///     If one of the properties is false - we won't be able to receive push notifications.
    /// </summary>
    public class PushNotificationRegistrationResult
    {
        public PushNotificationRegistrationResult()
        {
        }

        public PushNotificationRegistrationResult(bool isRegisteredInSystem, bool isSavedOnServer)
        {
            IsRegisteredInSystem = isRegisteredInSystem;
            IsSavedOnServer = isSavedOnServer;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether value that indicates if we are registered to push notifications
        ///     in system with token stored in <see cref="IPushTokenStorageService"/>.
        /// </summary>
        public bool IsRegisteredInSystem { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether value that indicates if current token was saved on server.
        /// </summary>
        public bool IsSavedOnServer { get; set; }
    }
}
