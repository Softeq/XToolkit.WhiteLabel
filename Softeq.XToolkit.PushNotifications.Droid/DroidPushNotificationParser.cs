// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationParser : IPushNotificationParser
    {
        private const string StringResourceType = "string";

        /// <summary>
        ///     Gets custom key for title (stored inside <see cref="P:Firebase.Messaging.RemoteMessage.Data"/>).
        ///     Can be customized.
        /// </summary>
        protected virtual string DataTitleKey => "title";

        /// <summary>
        ///     Gets custom key for additional body (stored inside <see cref="P:Firebase.Messaging.RemoteMessage.Data"/>).
        ///     Can be customized.
        /// </summary>
        protected virtual string DataBodyKey => "body";

        /// <summary>
        ///     Gets custom key for additional data (stored inside <see cref="P:Firebase.Messaging.RemoteMessage.Data"/>).
        ///     Can be customized.
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

            pushNotification.Title = ParseNotificationTitleFromData(pushMessage, pushData);
            pushNotification.Body = ParseNotificationMessageFromData(pushMessage, pushData);

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

            pushNotification.AdditionalData = bundleNotification.GetString(DataKey)!;
            pushNotification.Type = ParseNotificationTypeFromBundle(bundleNotification);

            return pushNotification;
        }

        protected virtual string? ParseNotificationTitleFromData(
            RemoteMessage.Notification pushMessage,
            IDictionary<string, string> pushNotificationData)
        {
            if (pushMessage == null)
            {
                return GetStringFromDictionary(pushNotificationData, DataTitleKey);
            }
            else
            {
                string? title = null;
                if (!string.IsNullOrEmpty(pushMessage.TitleLocalizationKey))
                {
                    title = GetResourceString(pushMessage.TitleLocalizationKey, pushMessage.GetTitleLocalizationArgs());
                }

                return title ?? pushMessage.Title;
            }
        }

        protected virtual string? ParseNotificationMessageFromData(
            RemoteMessage.Notification pushMessage,
            IDictionary<string, string> pushNotificationData)
        {
            if (pushMessage == null)
            {
                return GetStringFromDictionary(pushNotificationData, DataBodyKey);
            }
            else
            {
                string? body = null;
                if (!string.IsNullOrEmpty(pushMessage.BodyLocalizationKey))
                {
                    body = GetResourceString(pushMessage.BodyLocalizationKey, pushMessage.GetBodyLocalizationArgs());
                }

                return body ?? pushMessage.Body;
            }
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values).
        /// </summary>
        /// <param name="pushNotificationData">
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"/>
        ///     which represents <see cref="P:Firebase.Messaging.RemoteMessage.Data"/>.
        /// </param>
        /// <returns>Notification type.</returns>
        protected virtual string ParseNotificationTypeFromData(IDictionary<string, string> pushNotificationData)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values).
        /// </summary>
        /// <param name="pushNotificationBundle">
        ///     <see cref="T:Android.OS.Bundle"/> which was obtained from extras when application was started from push notification.
        /// </param>
        /// <returns>Notification type.</returns>
        protected virtual string ParseNotificationTypeFromBundle(Bundle pushNotificationBundle)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Obtains string by key from the specified <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="data"><see cref="T:System.Collections.Generic.IDictionary`2"/> object.</param>
        /// <param name="key">Key string.</param>
        /// <returns>String stored under the specified key or <see langword="null"/>.</returns>
        protected string GetStringFromDictionary(IDictionary<string, string>? data, string key)
        {
            return data == null
                ? string.Empty
                : data.TryGetValue(key, out var value)
                    ? value
                    : string.Empty;
        }

        /// <summary>
        ///     Obtains localized string from Android resources by the specified key with the specified arguments.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <param name="args">Arguments.</param>
        /// <returns>Localized string from Android resources.</returns>
        protected string? GetResourceString(string resourceKey, string[] args)
        {
            var context = Application.Context;
            var resourceId = context.Resources?.GetIdentifier(resourceKey, StringResourceType, context.PackageName) ?? 0;
            if (resourceId != 0)
            {
                if (args != null)
                {
                    // Pavel Savik: This is the only way how it works, so, please, don't touch it.
                    var sanitizedArgs = args?.Where(s => s != null).Select(s => new Java.Lang.String(s)).Cast<Java.Lang.Object>().ToArray();
                    return context.GetString(resourceId, sanitizedArgs);
                }
                else
                {
                    return context.GetString(resourceId);
                }
            }

            return null;
        }
    }
}
