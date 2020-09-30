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
