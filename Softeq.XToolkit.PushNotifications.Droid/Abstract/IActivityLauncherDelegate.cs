// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    /// <summary>
    ///     Tracks when activity is launched and checks if extras contain push notification payload.
    /// </summary>
    public interface IActivityLauncherDelegate
    {
        /// <summary>
        ///     Callback called when application is launched by push notification.
        /// </summary>
        /// <param name="extras">Extra payload provided to the application on launch.</param>
        /// <returns><see langword="true"/> if extras are handled as push notification payload, <see langword="false"/> otherwise.</returns>
        bool TryHandlePushNotificationExtras(Bundle? extras);
    }
}
