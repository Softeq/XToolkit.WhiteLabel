// Developed by Softeq Development Corporation
// http://www.softeq.com

using Javax.Net.Ssl;
using Xamarin.Android.Net;

namespace Playground.Forms.Droid.CustomComponents
{
    // YP: Additional materials:
    // - Xamarin.Android Certificate Pinning: https://nicksnettravels.builttoroam.com/android-certificates/
    // - AndroidClientHandler sources: https://github.com/xamarin/xamarin-android/blob/60bc25e040e22679a2c09c9c7917194a8f459122/src/Mono.Android/Xamarin.Android.Net/AndroidClientHandler.cs
    // - Android Official: https://developer.android.com/training/articles/security-ssl
    internal class DroidIgnoreSslClientHandler : AndroidClientHandler
    {
        protected override SSLSocketFactory? ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
        {
            return (SSLSocketFactory?)SSLSocketFactory.Default;
        }

        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
        {
            return new IgnoreSslHostnameVerifier();
        }
    }

    internal class IgnoreSslHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string? hostname, ISSLSession? session)
        {
            return true;
        }
    }
}
