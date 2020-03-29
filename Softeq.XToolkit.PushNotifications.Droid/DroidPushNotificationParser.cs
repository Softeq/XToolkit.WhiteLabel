// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.OS;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationParser : IPushNotificationParser
    {
        /// <summary>
        ///     Custom key for title (stored inside RemoteMessage.Data). Can be customized.
        /// </summary>
        protected virtual string DataTitleKey => "title";

        /// <summary>
        ///     Custom key for addition body (stored inside RemoteMessage.Data). Can be customized.
        /// </summary>
        protected virtual string DataBodyKey => "body";

        /// <summary>
        ///     Custom key for addition data (stored inside RemoteMessage.Data). Can be customized.
        /// </summary>
        protected virtual string DataKey => "data";

        public PushNotificationModel Parse(object pushNotificationData)
        {
            var pushNotification = new PushNotificationModel();

            if (pushNotificationData is RemoteMessage remoteNotification)
            {
                pushNotification = ParseRemoteMessageNotification(remoteNotification);
            }
            else if (pushNotificationData is Bundle bundleNotification)
            {
                pushNotification = ParseBundleNotification(bundleNotification);
            }

            return pushNotification;
        }

        private PushNotificationModel ParseRemoteMessageNotification(RemoteMessage remoteNotification)
        {
            var pushNotification = new PushNotificationModel();

            var pushMessage = remoteNotification.GetNotification();
            var pushData = remoteNotification.Data;

            pushNotification.Title = pushMessage == null ? GetStringFromDictionary(pushData, DataTitleKey) : pushMessage.Title;
            pushNotification.Body = pushMessage == null ? GetStringFromDictionary(pushData, DataBodyKey) : pushMessage.Body;

            pushNotification.IsSilent = string.IsNullOrEmpty(pushNotification.Body);

            pushNotification.Type = ParseNotificationTypeFromData(pushData);
            pushNotification.AdditionalData = GetStringFromDictionary(pushData, DataKey);

            return pushNotification;
        }

        private PushNotificationModel ParseBundleNotification(Bundle bundleNotification)
        {
            var pushNotification = new PushNotificationModel();

            pushNotification.Title = bundleNotification.GetString(DataTitleKey); // if stored inside Data
            pushNotification.Body = bundleNotification.GetString(DataBodyKey); // if stored inside Data

            // If we are here it means that the user tapped on a notification thus it is definitely not silent
            pushNotification.IsSilent = false;

            pushNotification.AdditionalData = bundleNotification.GetString(DataKey);
            pushNotification.Type = ParseNotificationTypeFromBundle(bundleNotification);

            return pushNotification;
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values)
        /// </summary>
        /// <param name="pushNotificationData">IDictionary which represents RemoteMessage.Data</param>
        /// <returns></returns>
        protected virtual string ParseNotificationTypeFromData(IDictionary<string, string> pushNotificationData)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values)
        /// </summary>
        /// <param name="pushNotificationBundle">Bundle which was obtained from extras when application was started from push notification</param>
        /// <returns></returns>
        protected virtual string ParseNotificationTypeFromBundle(Bundle pushNotificationBundle)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Obtains string by key from IDictionary
        /// </summary>
        /// <param name="data">IDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>String stored under the specified key or null</returns>
        protected string GetStringFromDictionary(IDictionary<string, string>? data, string key)
        {
            return data == null
                ? string.Empty
                : data.TryGetValue(key, out var value)
                    ? value
                    : string.Empty;
        }
    }
}
