// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    ///     Class containing push notifications registration result.
    ///     <para/>
    ///     If one of the properties is <see langword="false"/> - we won't be able to receive push notifications.
    /// </summary>
    public class PushNotificationRegistrationResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PushNotificationRegistrationResult"/> class.
        /// </summary>
        public PushNotificationRegistrationResult()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PushNotificationRegistrationResult"/> class with the specified parameters.
        /// </summary>
        /// <param name="isRegisteredInSystem">
        ///     A value indicating whether we are registered to push notifications in system with token stored in
        ///     <see cref="IPushTokenStorageService"/>.
        /// </param>
        /// <param name="isSavedOnServer">
        ///     A value indicating whether current token was saved on server.
        /// </param>
        public PushNotificationRegistrationResult(bool isRegisteredInSystem, bool isSavedOnServer)
        {
            IsRegisteredInSystem = isRegisteredInSystem;
            IsSavedOnServer = isSavedOnServer;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether we are registered to push notifications in system with token stored in
        ///     <see cref="IPushTokenStorageService"/>.
        /// </summary>
        public bool IsRegisteredInSystem { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether current token was saved on server.
        /// </summary>
        public bool IsSavedOnServer { get; set; }
    }
}
