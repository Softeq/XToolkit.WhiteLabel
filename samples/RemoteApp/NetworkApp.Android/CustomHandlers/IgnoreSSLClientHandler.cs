// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Net;
using Javax.Net.Ssl;
using Xamarin.Android.Net;

namespace NetworkApp.Droid.CustomHandlers
{
    // YP: Additional materials:
    // - Xamarin.Android Certificate Pinning: https://nicksnettravels.builttoroam.com/android-certificates/
    // - AndroidClientHandler sources: https://github.com/xamarin/xamarin-android/blob/master/src/Mono.Android/Xamarin.Android.Net/AndroidClientHandler.cs
    // - Android Official: https://developer.android.com/training/articles/security-ssl
    internal class IgnoreSSLClientHandler : AndroidClientHandler
    {
        protected override SSLSocketFactory ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
        {
            return SSLCertificateSocketFactory.GetInsecure(0, null);
        }

        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
        {
            return new IgnoreSSLHostnameVerifier();
        }
    }

    internal class IgnoreSSLHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }
    }
}
