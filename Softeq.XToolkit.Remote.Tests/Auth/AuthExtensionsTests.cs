// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Auth
{
    public class AuthExtensionsTests
    {
        [Fact]
        public void WithSessionContext_NullHttpClientBuilder_ThrowsArgumentNullException()
        {
            var sessionContext = Substitute.For<ISessionContext>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                AuthExtensions.WithSessionContext(null!, sessionContext);
            });
        }

        [Fact]
        public void WithSessionContext_NullSessionContext_ThrowsArgumentNullException()
        {
            var httpClientBuilder = Substitute.For<IHttpClientBuilder>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                httpClientBuilder.WithSessionContext(null!);
            });
        }

        [Fact]
        public void WithSessionContext_SessionContext_ReturnsHttpHandlerWithNewDelegate()
        {
            var httpClientBuilder = Substitute.For<IHttpClientBuilder>();
            var sessionContext = Substitute.For<ISessionContext>();

            var result = httpClientBuilder.WithSessionContext(sessionContext);

            Assert.IsAssignableFrom<IHttpClientBuilder>(result);
            httpClientBuilder.Received().AddHandler(Arg.Any<DelegatingHandler>());
        }
    }
}
