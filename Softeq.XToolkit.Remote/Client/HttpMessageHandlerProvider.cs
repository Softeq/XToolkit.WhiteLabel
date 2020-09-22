// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using System.Reflection;

namespace Softeq.XToolkit.Remote.Client
{
    public class HttpMessageHandlerProvider
    {
        private static HttpMessageHandler? _cachedNativeHttpMessageHandler;

        public static HttpMessageHandler? CreateDefaultHandler()
        {
            if (_cachedNativeHttpMessageHandler != null)
            {
                return _cachedNativeHttpMessageHandler;
            }

            // HACK YP: need check, because linker can change assembly.
            // Sources: https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/HttpClient.DefaultHandler.cs#L5

            var createHandler = typeof(HttpClient).GetMethod("CreateDefaultHandler", BindingFlags.NonPublic | BindingFlags.Static);
            var nativeHandler = createHandler?.Invoke(null, null);
            _cachedNativeHttpMessageHandler = nativeHandler as HttpMessageHandler;

            return _cachedNativeHttpMessageHandler;
        }
    }
}
