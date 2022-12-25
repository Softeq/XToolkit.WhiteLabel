// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    /// <summary>
    ///     Tracks when activity is launched and checks if extras contain push notification payload.
    /// </summary>
    public interface IActivityLauncherDelegate
    {
        /// <summary>
        ///     Callback called when activity is launched by intent.
        /// </summary>
        /// <param name="intent">Launch intent.</param>
        /// <returns><see langword="true"/> if extras are handled as push notification payload, <see langword="false"/> otherwise.</returns>
        bool TryHandleLaunchIntent(Intent? intent);
    }
}
