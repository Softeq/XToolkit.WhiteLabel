// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    ///     Options of how to Unregister from Push Notifications.
    /// </summary>
    public enum PushNotificationsUnregisterOptions
    {
        /// <summary>
        ///     Unregister in system only, but do not unregister on server.
        /// </summary>
        InSystemOnly,

        /// <summary>
        ///     Unregister on server only, but do not unregister in system.
        /// </summary>
        OnServerOnly,

        /// <summary>
        ///     Unregister both in system and on server.
        /// </summary>
        InSystemAndOnServer
    }

    public static class PushNotificationsUnregisterOptionsExtensions
    {
        public static bool ShouldUnregisterInSystem(this PushNotificationsUnregisterOptions options) =>
            options == PushNotificationsUnregisterOptions.InSystemOnly
            || options == PushNotificationsUnregisterOptions.InSystemAndOnServer;

        public static bool ShouldUnregisterOnServer(this PushNotificationsUnregisterOptions options) =>
            options == PushNotificationsUnregisterOptions.OnServerOnly
            || options == PushNotificationsUnregisterOptions.InSystemAndOnServer;
    }
}
