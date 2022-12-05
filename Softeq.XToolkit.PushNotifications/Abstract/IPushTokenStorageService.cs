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
    }
}
