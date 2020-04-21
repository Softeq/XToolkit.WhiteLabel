// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public static class NSDataExtensions
    {
        /// <summary>
        ///     Converts Foundation.NSData value to its equivalent hexadecimal string representation.
        ///     Source: https://stackoverflow.com/a/58028222/5925490 .
        /// </summary>
        /// <param name="data">Byte buffer wrapper.</param>
        /// <returns>String representation.</returns>
        public static string AsString(this NSData data)
        {
            var bytes = data.ToArray();
            return BitConverter.ToString(bytes);
        }
    }
}
