// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    public class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        public DefaultHttpMessageHandlerBuilder()
            : this(HttpMessageHandlerProvider.CreateDefaultHandler())
        {
        }

        public DefaultHttpMessageHandlerBuilder(HttpMessageHandler? primaryHandler)
        {
            PrimaryHandler = primaryHandler;
            AdditionalHandlers = new List<DelegatingHandler>();
        }

        /// <inheritdoc />
        public override IList<DelegatingHandler> AdditionalHandlers { get; }

        /// <inheritdoc />
        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null)
            {
                throw new InvalidOperationException($"The '{nameof(PrimaryHandler)}' must not be null.");
            }

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}
