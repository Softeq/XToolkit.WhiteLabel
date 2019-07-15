// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    /// Result of push-notifications unregistering.
    /// </summary>
    public enum PushNotificationsUnregisterResult
    {
        /// <summary>
        /// Unregister completely failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Unregister failed during remote request.
        /// </summary>
        ServerFailed,

        /// <summary>
        /// Unregister completely success.
        /// </summary>
        Success
    }
}
