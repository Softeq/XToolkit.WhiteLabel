// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;

namespace Playground.Forms.iOS.CustomComponents
{
    internal static class IosIgnoreSslClientHelper
    {
        // YP: Additional materials:
        // - Xamarin.iOS Certificate Pinning: https://nicksnettravels.builttoroam.com/ios-certificate/
        // - NSUrlSessionHandler sources: https://github.com/xamarin/xamarin-macios/blob/edd8a2c8963063ca0ab3357e251944f4aa053fa8/src/Foundation/NSUrlSessionHandler.cs
        internal static HttpMessageHandler CreateHandler()
        {
            var nativeHandler = new NSUrlSessionHandler();

            nativeHandler.TrustOverrideForUrl = (sender, url, trust) =>
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
