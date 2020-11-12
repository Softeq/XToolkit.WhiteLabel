// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    /// <summary>
    ///     Information about which Activity and how should be opened on tap on Notification.
    /// </summary>
    public class NotificationIntentActivityInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationIntentActivityInfo"/> class.
        /// </summary>
        /// <param name="activityType">The type of the Activity that shall be opened.</param>
        /// <param name="doCreateParentStack">Value indicating whether parent stack should be created for this Activity.</param>
        public NotificationIntentActivityInfo(Type activityType, bool doCreateParentStack)
        {
            ActivityType = activityType;
            DoCreateParentStack = doCreateParentStack;
        }

        /// <summary>
        ///     Gets or sets the type of the Activity that shall be opened.
        /// </summary>
        public Type ActivityType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether parent stack should be created for this Activity.
        ///     <para/>
        ///     Note that to create parent stack or an activity in separate task you need to add some directives in manifest/activity declaration.
        ///     For more details <see href="https://developer.android.com/training/notify-user/navigation"/>.
        /// </summary>
        public bool DoCreateParentStack { get; set; }
    }
}
