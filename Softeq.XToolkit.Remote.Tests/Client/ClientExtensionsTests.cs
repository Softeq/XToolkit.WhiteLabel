// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client
{
    public class ClientExtensionsTests
    {
        [Fact]
        public void WithLogger_NullHttpClientBuilder_ThrowsArgumentNullException()
        {
            var logger = Substitute.For<ILogger>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                ClientExtensions.WithLogger(null!, logger);
            });
        }

        [Fact]
        public void WithLogger_NullLogger_ThrowsArgumentNullException()
        {
            var httpClientBuilder = Substitute.For<IHttpClientBuilder>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                httpClientBuilder.WithLogger(null!);
            });
        }

        [Fact]
        public void WithLogger_CorrectLogger_ReturnsHttpHandlerWithNewDelegate()
        {
            var httpClientBuilder = Substitute.For<IHttpClientBuilder>();
            var logger = Substitute.For<ILogger>();

            var result = httpClientBuilder.WithLogger(logger);

            Assert.IsAssignableFrom<IHttpClientBuilder>(result);
            httpClientBuilder.Received().AddHandler(Arg.Any<DelegatingHandler>());
        }
    }
}
