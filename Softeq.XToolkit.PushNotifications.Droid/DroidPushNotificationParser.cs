// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Firebase.Messaging;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationParser : IPushNotificationsParser
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

        /// <inheritdoc />
        public bool TryParse(RemoteMessage message, out PushNotificationModel parsedPushNotificationModel)
        {
            parsedPushNotificationModel = new PushNotificationModel();

            var pushMessage = message.GetNotification();
            var pushData = message.Data;

            if (!TryParseNotificationTypeFromData(pushData, out var notificationType))
            {
                return false;
            }

            parsedPushNotificationModel.Title = ParseNotificationTitleFromData(pushMessage, pushData);
            parsedPushNotificationModel.Body = ParseNotificationMessageFromData(pushMessage, pushData);

            parsedPushNotificationModel.IsSilent = string.IsNullOrEmpty(parsedPushNotificationModel.Body);

            parsedPushNotificationModel.Type = notificationType;
            parsedPushNotificationModel.AdditionalData = GetStringFromDictionary(pushData, DataKey);

            return true;
        }

        /// <inheritdoc />
        public bool TryParse(Bundle? bundle, out PushNotificationModel parsedPushNotificationModel)
        {
            parsedPushNotificationModel = new PushNotificationModel();

            if (bundle == null)
            {
                return false;
            }

            if (!TryParseNotificationTypeFromBundle(bundle, out var notificationType))
            {
                return false;
            }

            parsedPushNotificationModel.Title = bundle.GetString(DataTitleKey); // if stored inside Data
            parsedPushNotificationModel.Body = bundle.GetString(DataBodyKey); // if stored inside Data

            // If we are here it means that the user tapped on a notification thus it is definitely not silent
            parsedPushNotificationModel.IsSilent = false;

            parsedPushNotificationModel.AdditionalData = bundle.GetString(DataKey)!;
            parsedPushNotificationModel.Type = notificationType;

            return true;
        }

        /// <summary>
        ///     Obtains string by key from the specified <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="data"><see cref="T:System.Collections.Generic.IDictionary`2"/> object.</param>
        /// <param name="key">Key string.</param>
        /// <returns>String stored under the specified key or <see langword="null"/>.</returns>
        protected static string GetStringFromDictionary(IDictionary<string, string>? data, string key)
        {
            return data == null
                ? string.Empty
                : data.TryGetValue(key, out var value)
                    ? value
                    : string.Empty;
        }

        protected virtual string? ParseNotificationTitleFromData(
            RemoteMessage.Notification? pushMessage,
            IDictionary<string, string> pushNotificationData)
        {
            if (pushMessage == null)
            {
                return GetStringFromDictionary(pushNotificationData, DataTitleKey);
            }

            string? title = null;
            if (!string.IsNullOrEmpty(pushMessage.TitleLocalizationKey))
            {
                title = GetResourceString(pushMessage.TitleLocalizationKey, pushMessage.GetTitleLocalizationArgs());
            }

            return title ?? pushMessage.Title;
        }

        protected virtual string? ParseNotificationMessageFromData(
            RemoteMessage.Notification? pushMessage,
            IDictionary<string, string> pushNotificationData)
        {
            if (pushMessage == null)
            {
                return GetStringFromDictionary(pushNotificationData, DataBodyKey);
            }

            string? body = null;
            if (!string.IsNullOrEmpty(pushMessage.BodyLocalizationKey))
            {
                body = GetResourceString(pushMessage.BodyLocalizationKey, pushMessage.GetBodyLocalizationArgs());
            }

            return body ?? pushMessage.Body;
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values).
        /// </summary>
        /// <param name="pushNotificationData">
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"/>
        ///     which represents <see cref="P:Firebase.Messaging.RemoteMessage.Data"/>.
        /// </param>
        /// <param name="notificationType">Parsed notification type.</param>
        /// <returns><see langword="true"/> if notification type has been parsed, <see langword="false"/> otherwise.</returns>
        protected virtual bool TryParseNotificationTypeFromData(
            IDictionary<string, string> pushNotificationData,
            out string notificationType)
        {
            notificationType = string.Empty;
            return true;
        }

        /// <summary>
        ///     Obtains type string from push notification data (should be customized to provide app specific values).
        /// </summary>
        /// <param name="pushNotificationBundle">
        ///     <see cref="T:Android.OS.Bundle"/> which was obtained from extras when application was started from push notification.
        /// </param>
        /// <param name="notificationType">Parsed notification type.</param>
        /// <returns><see langword="true"/> if notification type has been parsed, <see langword="false"/> otherwise.</returns>
        protected virtual bool TryParseNotificationTypeFromBundle(Bundle pushNotificationBundle, out string notificationType)
        {
            notificationType = string.Empty;
            return true;
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
