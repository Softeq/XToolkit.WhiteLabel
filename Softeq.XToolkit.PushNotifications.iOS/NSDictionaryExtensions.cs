// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    // ReSharper disable once InconsistentNaming
    public static class NSDictionaryExtensions
    {
        /// <summary>
        ///     Obtains object by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>Object stored under the specified key or null</returns>
        public static NSObject GetObjectByKey(this NSDictionary dict, string key)
        {
            var nsKey = new NSString(key);
            if (dict != null && dict.ContainsKey(nsKey))
            {
                return dict.ObjectForKey(nsKey);
            }

            return null;
        }

        /// <summary>
        ///     Obtains NSDictionary by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>NSDictionary stored under the specified key or null</returns>
        public static NSDictionary GetDictionaryByKey(this NSDictionary dict, string key)
        {
            return GetObjectByKey(dict, key) as NSDictionary;
        }

        /// <summary>
        ///     Obtains string by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>String stored under the specified key or null</returns>
        public static string GetStringByKey(this NSDictionary dict, string key)
        {
            return GetObjectByKey(dict, key) as NSString;
        }

        /// <summary>
        ///     Obtains int by key from NSDictionary
        /// </summary>
        /// <param name="dict">NSDictionary object</param>
        /// <param name="key">Key string</param>
        /// <returns>Int stored under the specified key or null</returns>
        public static int GetIntByKey(this NSDictionary dict, string key)
        {
            return (GetObjectByKey(dict, key) as NSNumber)?.Int32Value ?? -1;
        }
    }
}
