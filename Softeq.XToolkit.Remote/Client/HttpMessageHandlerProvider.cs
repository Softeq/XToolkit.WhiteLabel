// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Reflection;

namespace Softeq.XToolkit.Remote.Client
{
    public class HttpMessageHandlerProvider
    {
        private static HttpMessageHandler? _cachedNativeHttpMessageHandler;

        /// <summary>
        ///     Creates default system <see cref="T:System.Net.Http.HttpClientHandler"/> instance, depends on the platform:
        ///     .NET Core (<see cref="T:System.Net.Http.HttpClientHandler"/>),
        ///     Apple (<see cref="T:System.Net.Http.NSUrlSessionHandler"/>),
        ///     Android (<see cref="T:Xamarin.Android.Net.AndroidClientHandler"/>).
        /// </summary>
        /// <remarks>
        ///     Uses reflection to create private default <see cref="T:System.Net.Http.HttpClientHandler"/> instance
        ///     from <see cref="T:System.Net.Http.HttpClient"/> class.
        ///
        ///     Also, see how to setup default http stack for:
        ///     <see href="https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/http-stack">iOS</see>
        ///     <see href="https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/http-stack?tabs=macos">Android</see>.
        /// </remarks>
        /// <returns>New instance of <see cref="T:System.Net.Http.HttpClientHandler"/>.</returns>
        public static HttpMessageHandler? CreateDefaultHandler()
        {
            if (!IsRunningOnMono)
            {
                return new HttpClientHandler();
            }

            if (_cachedNativeHttpMessageHandler != null)
            {
                return _cachedNativeHttpMessageHandler;
            }

            // HACK YP: need check, because linker can change assembly.
            // Sources: https://github.com/mono/mono/blob/6034bc00c432356a31208136b871ed152eb3f8d3/mcs/class/System.Net.Http/HttpClient.DefaultHandler.cs#L5

            var createHandler = typeof(HttpClient).GetMethod("CreateDefaultHandler", BindingFlags.NonPublic | BindingFlags.Static);
            var nativeHandler = createHandler?.Invoke(null, null);
            _cachedNativeHttpMessageHandler = nativeHandler as HttpMessageHandler;

            return _cachedNativeHttpMessageHandler;
        }

        private static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;
    }
}
