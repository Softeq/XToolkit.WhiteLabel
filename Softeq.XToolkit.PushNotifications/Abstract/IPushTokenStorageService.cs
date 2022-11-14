// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushTokenStorageService
    {
        /// <summary>
        ///     Gets or sets push token in custom storage.
        /// </summary>
        string PushToken { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we are subscribed to push notifications in system.
        /// </summary>
        bool IsTokenRegisteredInSystem { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we are subscribed to notifications on server.
        /// </summary>
        bool IsTokenSavedOnServer { get; set; }
    }
}
