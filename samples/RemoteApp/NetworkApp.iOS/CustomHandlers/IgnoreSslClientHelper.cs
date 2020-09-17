// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using Security;

namespace NetworkApp.iOS.CustomHandlers
{
    internal static class IgnoreSslClientHelper
    {
        // YP: Additional materials:
        // - Xamarin.iOS Certificate Pinning: https://nicksnettravels.builttoroam.com/ios-certificate/
        // - NSUrlSessionHandler sources: https://github.com/xamarin/xamarin-macios/blob/master/src/Foundation/NSUrlSessionHandler.cs
        internal static HttpMessageHandler CreateHandler()
        {
            var nativeHandler = new NSUrlSessionHandler();

            nativeHandler.TrustOverride = (NSUrlSessionHandler sender, SecTrust trust) =>
            {
                trust.Evaluate(out var error);

                if (error != null)
                {
                    Console.WriteLine(error.UserInfo);
                }

                return true;
            };

            // also need ignore invalid SSL domain in info.plist (ATS)

            return nativeHandler;
        }
    }
}
