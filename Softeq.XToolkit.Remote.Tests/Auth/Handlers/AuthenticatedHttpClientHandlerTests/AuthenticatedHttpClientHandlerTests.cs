// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Remote.Auth.Handlers;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.AuthenticatedHttpClientHandlerTests
{
    public class AuthenticatedHttpClientHandlerTests
    {
        private const string TestAuthHeaderSchema = "test-auth";
        private const string TestAuthTokenValue = "test-token-xxx";

        private readonly TestAuthenticatedHttpClientHandler _handler;
        private readonly HttpRequestMessage _request;

        public AuthenticatedHttpClientHandlerTests()
        {
            _request = new HttpRequestMessage();

            var accessTokenProvider = Substitute.For<Func<Task<string>>>();
            accessTokenProvider.Invoke().Returns(Task.FromResult(TestAuthTokenValue));

            var innerHandler = Substitute.For<HttpMessageHandler>();

            _handler = new TestAuthenticatedHttpClientHandler(accessTokenProvider)
            {
                InnerHandler = innerHandler
            };
        }

        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new AuthenticatedHttpClientHandler(null!);
            });
        }

        [Fact]
        public async Task SendAsync_WithoutAuthorizationHeader_ExecutesWithoutAccessToken()
        {
            await _handler.PublicSendAsync(_request, CancellationToken.None);
        }

        [Fact]
        public async Task SendAsync_WithAuthorizationHeader_ExecutesWithAccessToken()
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue(TestAuthHeaderSchema);

            await _handler.PublicSendAsync(_request, CancellationToken.None);
            var resultToken = _request.Headers.Authorization.Parameter;

            Assert.Equal(TestAuthTokenValue, resultToken);
        }
    }
}
