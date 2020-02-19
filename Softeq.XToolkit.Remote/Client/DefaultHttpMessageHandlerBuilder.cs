// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Softeq.XToolkit.Remote.Client
{
    public class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        public DefaultHttpMessageHandlerBuilder()
            : this(CreateDefaultHandler())
        {
        }

        public DefaultHttpMessageHandlerBuilder(HttpMessageHandler primaryHandler)
        {
            PrimaryHandler = primaryHandler;
        }

        /// <inheritdoc />
        public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();

        /// <inheritdoc />
        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null)
            {
                var message = string.Format("The '{0}' must not be null.", nameof(PrimaryHandler));
                throw new InvalidOperationException(message);
            }

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}
