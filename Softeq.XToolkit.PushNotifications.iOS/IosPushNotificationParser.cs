// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosPushNotificationParser : IPushNotificationParser
    {
        /// <summary>
        /// A root dictionary containing one or more additional Apple-defined keys instructing the system how to handle notification.
        /// Don't add your own custom keys to the aps dictionary; APNs ignores custom keys. Instead, add your custom keys as peers of the aps dictionary
        /// </summary>
        protected const string ApsKey = "aps";

        /// <summary>
        /// Flag inside aps. The information for displaying an alert. A dictionary is recommended.
        /// If you specify a string, the alert displays your string as the body text.
        /// </summary>
        protected const string AlertKey = "alert";

        /// <summary>
        /// Flag inside alert. The title of the notification.
        /// </summary>
        protected const string TitleKey = "title";

        /// <summary>
        /// Flag inside alert. The content of the alert message.
        /// </summary>
        protected const string BodyKey = "body";

        /// <summary>
        /// Flag inside aps. The background notification flag. To perform a silent background update,
        /// specify the value 1 and don't include the alert, badge, or sound keys in your payload.
        /// </summary>
        protected const string ContentAvailableKey = "content-available";

        /// <summary>
        /// Custom key for addition data. Can be customized.
        /// </summary>
        protected virtual string DataKey => "data";

        public virtual PushNotificationModel Parse(object pushNotificationData)
        {
            var dictionary = (NSDictionary) pushNotificationData;
            var pushNotification = new PushNotificationModel();

            var aps = dictionary.GetDictionaryByKey(ApsKey);

            var alertObject = aps.GetObjectByKey(AlertKey);

            string title = null;
            string body = null;

            if (alertObject is NSDictionary alertDictionary)
            {
                title = alertDictionary.GetStringByKey(TitleKey);
                body = alertDictionary.GetStringByKey(BodyKey);
            }
            else if (alertObject is NSString str)
            {
                body = str;
            }
            pushNotification.Title = title;
            pushNotification.Body = body;

            pushNotification.IsSilent = aps.GetIntByKey(ContentAvailableKey) == 1;

            var additionalData = dictionary.GetStringByKey(DataKey);
            pushNotification.AdditionalData = additionalData;

            pushNotification.Type = ParseNotificationType(dictionary, aps, additionalData);

            return pushNotification;
        }

        /// <summary>
        /// Obtains type string from push notification data (should be customized to provide app specific values)
        /// </summary>
        /// <param name="pushNotification">Initial push notification dictionary</param>
        /// <param name="aps">Dictionary stored inside 'aps' tag</param>
        /// <param name="data">Custom data part of the notification</param>
        /// <returns></returns>
        protected virtual string ParseNotificationType(NSDictionary pushNotification, NSDictionary aps, string data)
        {
            return string.Empty;
        }
    }
}
