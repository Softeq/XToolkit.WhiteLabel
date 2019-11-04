// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Runtime.InteropServices;
using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public static class NSDataExtensions
    {
        // https://stackoverflow.com/questions/58027344/how-to-get-device-token-in-ios-13-with-xamarin/58028222#58028222
        public static string AsString(this NSData data)
        {
            var result = new byte[data.Length];
            Marshal.Copy(data.Bytes, result, 0, (int) data.Length);
            var token = BitConverter.ToString(result);
            return token;
        }
    }
}
