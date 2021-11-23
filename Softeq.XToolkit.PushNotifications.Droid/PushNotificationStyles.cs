// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Media;
using Android.Net;
using AndroidX.Core.App;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class PushNotificationStyles
    {
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

        /// <summary>
        ///     Gets or sets notification Id. Notifications with the same id will be replaced.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether if true, notification will be automatically removed when the user taps it.
        /// </summary>
        public bool AutoCancel { get; set; }

        public Uri SoundUri { get; set; }
        public NotificationPriority Priority { get; set; }
        public NotificationCompat.Style Style { get; set; }
        public int IconRes { get; set; }
        public int IconArgbColor { get; set; }
    }
}
