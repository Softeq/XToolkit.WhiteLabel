// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Remote.Auth.Handlers;
using Softeq.XToolkit.Remote.Exceptions;
using Xunit;
using static Softeq.XToolkit.Remote.Tests.Auth.Handlers.AuthenticatedHttpClientHandlerTestsDataProvider;
using static Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests.RefreshTokenHttpClientHandlerDataProvider;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests
{
    [SuppressMessage("ReSharper", "ConvertToLambdaExpression", Justification = "More readable")]
    public class RefreshTokenHttpClientHandlerTests
    {
        private readonly HttpRequestMessage _request;
        private readonly Func<Task<string>> _accessTokenProvider;
        private readonly Func<Task> _refreshTokenProvider;
        private readonly TestRefreshTokenHttpClientHandler _handler;
        private readonly object? _mockInternalSendAsync;

        public RefreshTokenHttpClientHandlerTests()
        {
            _request = new HttpRequestMessage();

            _accessTokenProvider = Substitute.For<Func<Task<string>>>();
            _accessTokenProvider.Invoke().Returns(CreateAccessTokenResult());

            _refreshTokenProvider = Substitute.For<Func<Task>>();

            var innerHandler = Substitute.For<HttpMessageHandler>();
            _mockInternalSendAsync = innerHandler.Protected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());

            _handler = new TestRefreshTokenHttpClientHandler(_accessTokenProvider, _refreshTokenProvider)
            {
                InnerHandler = innerHandler
            };
        }

        [Fact]
        public void Ctor_NullAccessToken_ThrowsArgumentNullException()
        {
            var getRefreshToken = Substitute.For<Func<Task>>();

            Assert.Throws<ArgumentNullException>(() => new RefreshTokenHttpClientHandler(null!, getRefreshToken));
        }

        [Fact]
        public void Ctor_NullRefreshToken_ThrowsArgumentNullException()
        {
            var getAccessToken = Substitute.For<Func<Task<string>>>();

            Assert.Throws<ArgumentNullException>(() => new RefreshTokenHttpClientHandler(getAccessToken, null!));
        }

        [Fact]
        public async Task SendAsync_WithoutAuthorizationHeaderCancelled_ExecutesWithoutAccessToken()
        {
            var cancellationToken = CreateCancellationToken();

            await _handler.PublicSendAsync(_request, cancellationToken);
        }

        [Fact]
        public async Task SendAsync_WithAuthorizationHeaderCancelled_ThrowsOperationCanceledException()
        {
            _request.Headers.Authorization = CreateAuthHeader();
            var cancellationToken = CreateCancellationToken();

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
            {
                return _handler.PublicSendAsync(_request, cancellationToken);
            });
        }

        [Fact]
        public async Task SendAsync_WithoutAuthorizationHeaderAndSuccessResponse_ExecutesWithoutAccessToken()
        {
            _mockInternalSendAsync.Returns(Task.FromResult(new HttpResponseMessage()));

            await _handler.PublicSendAsync(_request, CancellationToken.None);
        }

        [Fact]
        public async Task SendAsync_WithAuthorizationHeaderAndUnauthorizedResponse_ThrowsExpiredRefreshTokenException()
        {
            _request.Headers.Authorization = CreateAuthHeader();
            _mockInternalSendAsync.Returns(CreateUnauthorizedMessage());

            await Assert.ThrowsAsync<ExpiredRefreshTokenException>(() =>
            {
                return _handler.PublicSendAsync(_request, CancellationToken.None);
            });
            await _accessTokenProvider.Received(2).Invoke();
            await _refreshTokenProvider.Received(1).Invoke();
        }

        [Fact]
        public async Task SendAsync_WithAuthorizationHeaderWithInternalException_ThrowsException()
        {
            _request.Headers.Authorization = CreateAuthHeader();
            _mockInternalSendAsync.Returns(CreateMessageWithInternalException());

            await Assert.ThrowsAsync<Exception>(() =>
            {
                return _handler.PublicSendAsync(_request, CancellationToken.None);
            });
        }
    }
}
