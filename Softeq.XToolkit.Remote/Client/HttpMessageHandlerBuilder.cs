// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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
        ///     Gets or sets the primary <see cref="T:System.Net.Http.HttpMessageHandler"/>.
        /// </summary>
        public virtual HttpMessageHandler? PrimaryHandler { get; set; }

        /// <summary>
        ///     Gets a list of additional <see cref="T:System.Net.Http.DelegatingHandler"/> instances used to configure an
        ///     <see cref="T:System.Net.Http.HttpClient"/> pipeline.
        /// </summary>
        public abstract IList<DelegatingHandler> AdditionalHandlers { get; }

        /// <summary>
        ///     Adds a <see cref="T:System.Net.Http.HttpMessageHandler"/> to the chain of handlers.
        /// </summary>
        /// <param name="handler">Handler.</param>
        /// <returns>Current builder.</returns>
        public HttpMessageHandlerBuilder AddHandler(DelegatingHandler handler)
        {
            AdditionalHandlers.Add(handler);

            return this;
        }

        /// <summary>
        ///     Creates an <see cref="T:System.Net.Http.HttpMessageHandler"/>. Adds <see cref="T:System.Net.Http.DelegatingHandler"/> as the last link of the chain.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Net.Http.HttpMessageHandler"/> built from the <see cref="PrimaryHandler"/> and
        ///     <see cref="AdditionalHandlers"/>.
        /// </returns>
        public abstract HttpMessageHandler Build();

        protected static HttpMessageHandler CreateHandlerPipeline(
            HttpMessageHandler primaryHandler,
            IList<DelegatingHandler> additionalHandlers)
        {
            // This is similar to https://github.com/aspnet/AspNetWebStack/blob/master/src/System.Net.Http.Formatting/HttpClientFactory.cs#L58
            // but we don't want to take that package as a dependency.
            if (primaryHandler == null)
            {
                throw new ArgumentNullException(nameof(primaryHandler));
            }

            if (additionalHandlers == null)
            {
                throw new ArgumentNullException(nameof(additionalHandlers));
            }

            var next = primaryHandler;
            for (var i = additionalHandlers.Count - 1; i >= 0; i--)
            {
                var handler = additionalHandlers[i];
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

                handler.InnerHandler = next;
                next = handler;
            }

            return next;
        }
    }
}
