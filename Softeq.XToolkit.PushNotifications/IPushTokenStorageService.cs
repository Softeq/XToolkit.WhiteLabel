// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushTokenStorageService
    {
        /// <summary>
        /// Property to get and set push token in custom storage
        /// </summary>
        string PushToken { get; set; }
    }
}
