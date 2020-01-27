// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    ///     Class containing push notifications registration result
    ///     If one of the properties is false - we won't be able to receive push notifications
    /// </summary>
    public class PushNotificationRegistrationResult
    {
        /// <summary>
        ///     Value that indicates if we are registered to push notifications in system with token stored in
        ///     IPushTokenStorageService
        /// </summary>
        public bool IsRegisteredInSystem { get; set; }

        /// <summary> Value that indicates if current token was saved on server </summary>
        public bool IsSavedOnServer { get; set; }

        public PushNotificationRegistrationResult() { }

        public PushNotificationRegistrationResult(bool isRegisteredInSystem, bool isSavedOnServer)
        {
            IsRegisteredInSystem = isRegisteredInSystem;
            IsSavedOnServer = isSavedOnServer;
        }
    }
}
