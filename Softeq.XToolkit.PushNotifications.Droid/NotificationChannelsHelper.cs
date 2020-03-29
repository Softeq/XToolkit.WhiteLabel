// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    /// <summary>
    /// A helper class to perform some actions with Notification Channels,
    /// can be used to manage Channels dynamically to reflect choices made by users of your app
    /// </summary>
    public static class NotificationChannelsHelper
    {
        /// <summary>
        /// Create and possibly configure a Notification Channel
        /// </summary>
        /// <param name="channelId">Id for the channel</param>
        /// <param name="channelName">Name for the channel (should be localized, don't forget to update the channel when Locale changes)</param>
        /// <param name="channelImportance">Channel importance</param>
        /// <param name="configureChannelAction">Action to additionally configure channel before registration (set description, group, etc.)</param>
        public static void CreateNotificationChannel(string channelId, string channelName, NotificationImportance channelImportance,
            Action? configureChannelAction = null)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(channelId, channelName, channelImportance);
                configureChannelAction?.Invoke();
                NotificationManager.FromContext(Application.Context).CreateNotificationChannel(channel);
            }
        }

        /// <summary>
        /// Delete a Notification Channel
        /// </summary>
        /// <param name="channelId">Id for the channel</param>
        public static void DeleteNotificationChannel(string channelId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationManager.FromContext(Application.Context).DeleteNotificationChannel(channelId);
            }
        }

        /// <summary>
        /// Create a Notification Channel Group
        /// Afterwards you can set this group to a notification channel with SetGroup(groupId) prior to it's registration in NotificationManager
        /// </summary>
        /// <param name="groupId">Id for the group</param>
        /// <param name="groupName">Name for the group (should be localized)</param>
        /// <param name="description">Optional parameter to specify Description for this group</param>
        public static void CreateNotificationChannelGroup(string groupId, string groupName, string? description = null)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var group = new NotificationChannelGroup(groupId, groupName);
                if (description != null)
                {
                    group.Description = description;
                }
                NotificationManager.FromContext(Application.Context).CreateNotificationChannelGroup(group);
            }
        }

        /// <summary>
        /// Delete a Notification Channel Group
        /// </summary>
        /// <param name="groupId">Id for the group</param>
        public static void DeleteNotificationChannelGroup(string groupId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationManager.FromContext(Application.Context).DeleteNotificationChannelGroup(groupId);
            }
        }
    }
}
