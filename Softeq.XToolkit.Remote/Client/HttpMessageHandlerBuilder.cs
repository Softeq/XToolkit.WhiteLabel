// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    ///     A builder abstraction for configuring <see cref="T:System.Net.Http.HttpMessageHandler"/> instances.
    /// </summary>
    /// <remarks>
    ///     Callers should retrieve a new instance for each <see cref="T:System.Net.Http.HttpMessageHandler"/> to be created.
    ///     Implementors should expect each instance to be used a single time.
    /// </remarks>
    public abstract class HttpMessageHandlerBuilder
    {
        /// <summary>
        ///     Gets or sets the primary <see cref="T:System.Net.Http.HttpMessageHandler"/> that
        ///     represents the destination of the HTTP message channel.
        /// </summary>
        public HttpMessageHandler PrimaryHandler { get; set; } = null!;

        /// <summary>
        ///     Gets a list of additional <see cref="T:System.Net.Http.DelegatingHandler"/> instances used to configure an
        ///     <see cref="T:System.Net.Http.HttpClient"/> pipeline.
        /// </summary>
        public IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();

        /// <summary>
        ///     Adds a <see cref="T:System.Net.Http.HttpMessageHandler"/> to the end of the handlers list.
        /// </summary>
        /// <param name="handler">Handler.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="handler"/> parameter is <see langword="null"/>.
        /// </exception>
        /// <returns>Current builder.</returns>
        public HttpMessageHandlerBuilder AddHandler(DelegatingHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            AdditionalHandlers.Add(handler);

            return this;
        }

        /// <summary>
        ///     Creates an <see cref="T:System.Net.Http.HttpMessageHandler"/>.
        ///     Adds <see cref="T:System.Net.Http.DelegatingHandler"/> as the last link of the chain.
        ///
        ///     The handlers are invoked in a top-down fashion. That is, the first entry is invoked first for
        ///     an outbound request message but last for an inbound response message.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Net.Http.HttpMessageHandler"/> built from the <see cref="PrimaryHandler"/> and
        ///     <see cref="AdditionalHandlers"/>.
        /// </returns>
        public abstract HttpMessageHandler Build();

        /// <summary>
        ///     Creates an instance of an <see cref="T:System.Net.Http.HttpMessageHandler"/>
        ///     using the <see cref="T:System.Net.Http.DelegatingHandler"/> instances
        ///     provided by <paramref name="additionalHandlers"/>.
        ///
        ///     The resulting pipeline can be used to manually create <see cref="HttpClient"/>
        ///     or <see cref="T:System.Net.Http.HttpMessageInvoker"/> instances with customized message handlers.
        /// </summary>
        /// <param name="primaryHandler">The inner handler represents the destination of the HTTP message channel.</param>
        /// <param name="additionalHandlers">
        ///     An ordered list of <see cref="T:System.Net.Http.DelegatingHandler"/> instances to be invoked as part
        ///     of sending an <see cref="T:System.Net.Http.HttpRequestMessage"/> and receiving
        ///     an <see cref="T:System.Net.Http.HttpResponseMessage"/>.
        ///
        ///     The handlers are invoked in a top-down fashion. That is, the first entry is invoked first for
        ///     an outbound request message but last for an inbound response message.
        /// </param>
        /// <returns>The HTTP message channel.</returns>
        protected static HttpMessageHandler CreateHandlerPipeline(
            HttpMessageHandler primaryHandler,
            IList<DelegatingHandler>? additionalHandlers)
        {
            // This is similar to https://github.com/aspnet/AspNetWebStack/blob/1a987f82d648a95aabed7d35e4c2bee17185c44d/src/System.Net.Http.Formatting/HttpClientFactory.cs#L58
            // but we don't want to take that package as a dependency.
            if (primaryHandler == null)
            {
                throw new ArgumentNullException(nameof(primaryHandler));
            }

            if (additionalHandlers == null || additionalHandlers.Count == 0)
            {
                return primaryHandler;
            }

            // Wire handlers up in reverse order starting with the primary handler
            HttpMessageHandler pipeline = primaryHandler;
            IEnumerable<DelegatingHandler> reversedHandlers = additionalHandlers.Reverse();
            foreach (DelegatingHandler handler in reversedHandlers)
            {
                if (handler == null)
                {
                    throw new InvalidOperationException($"The '{nameof(handler)}' must not contain a null entry.");
                }

                // Checking for this allows us to catch cases where someone has tried to re-use a handler. That really won't
                // work the way you want and it can be tricky for callers to figure out.
                if (handler.InnerHandler != null)
                {
                    var message = $"The '{nameof(DelegatingHandler.InnerHandler)}' property must be null. " +
                      $"'{nameof(DelegatingHandler)}' instances provided to '{nameof(HttpMessageHandlerBuilder)}' must not be reused or cached." +
                      $"{Environment.NewLine}Handler: '{handler}'";

                    throw new InvalidOperationException(message);
                }

                handler.InnerHandler = pipeline;
                pipeline = handler;
            }

            return pipeline;
        }
    }
}
