// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IActivityLauncherDelegate"/> interface for Android platform.
    /// </summary>
    public sealed class ActivityLauncherDelegate : IActivityLauncherDelegate
    {
        private readonly IDroidPushNotificationsConsumer _pushNotificationsConsumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLauncherDelegate"/> class.
        /// </summary>
        /// <param name="pushNotificationsConsumer">Consumer of the push notification related callbacks.</param>
        public ActivityLauncherDelegate(IDroidPushNotificationsConsumer pushNotificationsConsumer)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
        }

        /// <inheritdoc />
        public bool TryHandleLaunchIntent(Intent? intent)
        {
            return intent != null && _pushNotificationsConsumer.TryHandlePushNotificationIntent(intent);
        }
    }
}
