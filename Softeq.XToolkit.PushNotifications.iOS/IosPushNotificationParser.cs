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
        private const string ApsKey = "aps";

        /// <summary>
        /// Flag inside aps. The information for displaying an alert. A dictionary is recommended.
        /// If you specify a string, the alert displays your string as the body text.
        /// </summary>
        private const string AlertKey = "alert";

        /// <summary>
        /// Flag inside alert. The title of the notification.
        /// </summary>
        private const string TitleKey = "title";

        /// <summary>
        /// Flag inside alert. The content of the alert message.
        /// </summary>
        private const string BodyKey = "body";

        /// <summary>
        /// Flag inside aps. The background notification flag. To perform a silent background update,
        /// specify the value 1 and don't include the alert, badge, or sound keys in your payload.
        /// </summary>
        private const string ContentAvailableKey = "content-available";

        /// <summary>
        /// Custom key for addition data. Can be customized.
        /// </summary>
        protected virtual string DataKey => "data";

        public PushNotificationModel Parse(object pushNotificationData)
        {
            var dictionary = (NSDictionary) pushNotificationData;
            var pushNotification = new PushNotificationModel();

            var aps = GetDictionaryByKey(dictionary, ApsKey);

            var alertObject = GetObjectByKey(aps, AlertKey);

            string title = null;
            string body = null;

            if (alertObject is NSDictionary alertDictionary)
            {
                title = GetStringByKey(alertDictionary, TitleKey);
                body = GetStringByKey(alertDictionary, BodyKey);
            }
            else if (alertObject is NSString)
            {
                body = alertObject as NSString;
            }
            pushNotification.Title = title;
            pushNotification.Body = body;

            pushNotification.IsSilent = GetIntByKey(aps, ContentAvailableKey) == 1;

            var additinalData = GetObjectByKey(dictionary, DataKey);
            pushNotification.AdditionalData = additinalData;

            pushNotification.Type = ParseNotificationType(dictionary, aps, additinalData);

            return pushNotification;
        }

        /// <summary>
        /// Obtains type string from push notification data (should be customized to provide app specific values)
        /// </summary>
        /// <param name="pushNotification">Initial push notification dictionary</param>
        /// <param name="aps">Dictionary stored inside 'aps' tag</param>
        /// <param name="data">Custom data part of the notification</param>
        /// <returns></returns>
        protected virtual string ParseNotificationType(NSDictionary pushNotification, NSDictionary aps, object data)
        {
            return string.Empty;
        }

        /// <summary>
        /// Obtains object by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>Object stored under the specified key or null</returns>
        protected NSObject GetObjectByKey(NSDictionary dict, string key)
        {
            var nsKey = new NSString(key);
            if (dict != null && dict.ContainsKey(nsKey))
            {
                return dict.ObjectForKey(nsKey);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtains NSDictionary by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>NSDictionary stored under the specified key or null</returns>
        protected NSDictionary GetDictionaryByKey(NSDictionary dict, string key)
            => GetObjectByKey(dict, key) as NSDictionary;

        /// <summary>
        /// Obtains string by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>String stored under the specified key or null</returns>
        protected string GetStringByKey(NSDictionary dict, string key)
            => GetObjectByKey(dict, key) as NSString;

        /// <summary>
        /// Obtains int by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>Int stored under the specified key or null</returns>
        protected int GetIntByKey(NSDictionary dict, string key)
            => (GetObjectByKey(dict, key) as NSNumber)?.Int32Value ?? -1;
    }
}
