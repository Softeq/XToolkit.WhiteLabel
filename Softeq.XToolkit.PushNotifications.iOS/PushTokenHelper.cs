using System;
using System.Runtime.InteropServices;
using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public static class PushTokenHelper
    {
        // https://stackoverflow.com/questions/58027344/how-to-get-device-token-in-ios-13-with-xamarin/58028222#58028222
        public static string ParseDeviceToken(NSData deviceToken)
        {
            var result = new byte[deviceToken.Length];
            Marshal.Copy(deviceToken.Bytes, result, 0, (int) deviceToken.Length);
            var token = BitConverter.ToString(result).Replace("-", "");
            return token;
        }
    }
}
