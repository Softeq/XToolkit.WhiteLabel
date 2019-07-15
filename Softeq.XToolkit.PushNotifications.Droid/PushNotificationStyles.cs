// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Media;
using Android.Net;
using Android.Support.V4.App;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class PushNotificationStyles
    {
        /// <summary>
        /// Notification Id. Notifications with the same id will be replaced.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// If true, notification will be automatically removed when the user taps it
        /// </summary>
        public bool AutoCancel { get; }

        public Uri SoundUri { get; }
        public NotificationPriority Priority { get; }
        public NotificationCompat.Style Style { get; }
        public int IconRes { get; }
        public int IconArgbColor { get; }

        public PushNotificationStyles(NotificationCompat.Style style)
        {
            Id = 0;
            AutoCancel = true;
            SoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            Priority = NotificationPriority.Default;
            Style = style;
            IconRes = Application.Context.ApplicationInfo.Icon;
            IconArgbColor = Notification.ColorDefault;
        }
    }
}
