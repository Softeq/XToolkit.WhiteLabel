// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushTokenStorageService
    {
        /// <summary>
        ///     Property to get and set push token in custom storage
        /// </summary>
        string PushToken { get; set; }

        /// <summary>
        ///     Value to understand if we are subscribed to push notifications in system
        /// </summary>
        bool IsTokenRegisteredInSystem { get; set; }

        /// <summary>
        ///     Value to understand if we are subscribed to notifications on server
        /// </summary>
        bool IsTokenSavedOnServer { get; set; }
    }
}
