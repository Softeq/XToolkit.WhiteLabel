using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Softeq.XToolkit.Remote.Client
{
    public class HttpHandlerBuilder
    {
        private readonly IList<DelegatingHandler> _handlers = new List<DelegatingHandler>();
        private readonly HttpMessageHandler _rootHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandlerBuilder"/> class.
        /// </summary>
        public HttpHandlerBuilder() : this(CreateDefaultHandler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandlerBuilder"/> class.
        /// </summary>
        /// <param name="rootHandler">Root handler.</param>
        public HttpHandlerBuilder(HttpMessageHandler rootHandler)
        {
            _rootHandler = rootHandler;
        }

        /// <summary>
        /// Adds a <see cref="HttpMessageHandler"/> to the chain of handlers.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public HttpHandlerBuilder AddHandler(DelegatingHandler handler)
        {
            if (_handlers.Any())
            {
                _handlers.Last().InnerHandler = handler;
            }

            _handlers.Add(handler);

            return this;
        }

        /// <summary>
        /// Adds <see cref="DelegatingHandler"/> as the last link of the chain.
        /// </summary>
        /// <returns></returns>
        public HttpMessageHandler Build()
        {
            if (_handlers.Any())
            {
                _handlers.Last().InnerHandler = _rootHandler;
                return _handlers.FirstOrDefault();
            }

            return _rootHandler;
        }

        private static HttpMessageHandler CreateDefaultHandler()
        {
            // HACK YP: need check, because linker can change assembly.
            // Sources: https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/HttpClient.DefaultHandler.cs#L5

            var method = typeof(HttpClient).GetMethod("CreateDefaultHandler", BindingFlags.NonPublic | BindingFlags.Static);
            return method?.Invoke(null, null) as HttpMessageHandler;
        }
    }
}
