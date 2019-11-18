// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    /// <summary>
    /// Information about which Activity and how should be opened on tap on Notification
    /// </summary>
    public class NotificationIntentActivityInfo
    {
        /// <summary>
        /// Type of the Activity that shall be opened
        /// </summary>
        public Type ActivityType { get; set; }

        /// <summary>
        /// The value indicating whether parent stack should be created for this Activity
        /// Note that to create parent stack or an activity in separate task you need to add some directives in manifest/activity declaration.
        /// For more details <see cref="https://developer.android.com/training/notify-user/navigation"/>
        /// </summary>
        public bool DoCreateParentStack { get; set; }

        public NotificationIntentActivityInfo(Type activityType, bool doCreateParentStack)
        {
            ActivityType = activityType;
            DoCreateParentStack = doCreateParentStack;
        }
    }
}
