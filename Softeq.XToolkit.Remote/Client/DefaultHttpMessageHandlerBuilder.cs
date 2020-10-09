// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    public class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHttpMessageHandlerBuilder"/> class
        ///     with default primary handler provided by <see cref="HttpMessageHandlerProvider"/>.
        /// </summary>
        public DefaultHttpMessageHandlerBuilder()
            : this(HttpMessageHandlerProvider.CreateDefaultHandler()!)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHttpMessageHandlerBuilder"/> class.
        /// </summary>
        /// <param name="primaryHandler">Primary HttpMessageHandler.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the <paramref name="primaryHandler"/> is <see langword="null"/>.
        /// </exception>
        public DefaultHttpMessageHandlerBuilder(HttpMessageHandler primaryHandler)
        {
            PrimaryHandler = primaryHandler ?? throw new ArgumentNullException(nameof(primaryHandler));
        }

        /// <inheritdoc />
        public override HttpMessageHandler Build()
        {
            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}
