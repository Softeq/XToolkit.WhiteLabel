// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;
using Softeq.XToolkit.Remote.Primitives;
using Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client.Handlers.HttpDiagnosticsHandlerTests
{
    public class HttpDiagnosticsHandlerTests
    {
        private readonly HttpRequestMessage _request;
        private readonly ILogger _logger;
        private readonly object? _mockInternalSendAsync;
        private readonly HttpMessageHandler _innerHandler;

        public HttpDiagnosticsHandlerTests()
        {
            _request = new HttpRequestMessage
            {
                Content = new StringContent("test data")
            };

            _logger = Substitute.For<ILogger>();

            _innerHandler = Substitute.For<HttpMessageHandler>();
            _mockInternalSendAsync = _innerHandler.Protected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());
            _mockInternalSendAsync.Returns(Task.FromResult(new HttpResponseMessage()));
        }

        private string RequestTitle => Arg.Is<string>(x => x.Contains("Request:"));
        private string RequestBodyTitle => Arg.Is<string>(x => x.Contains("Request Content"));
        private string ResponseTitle => Arg.Is<string>(x => x.Contains("Response:"));
        private string ResponseBodyTitle => Arg.Is<string>(x => x.Contains("Response Content"));

        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new HttpDiagnosticsHandler(null!, LogVerbosity.Unspecified);
            });
        }

        [Fact]
        public async Task SendAsync_WithDefaultVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.Unspecified);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(RequestTitle);
            _logger.Received(1).Debug(RequestBodyTitle);
            _logger.Received(1).Debug(ResponseTitle);
            _logger.Received(1).Debug(ResponseBodyTitle);
        }

        [Fact]
        public async Task SendAsync_WithNoneVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.None);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.DidNotReceive().Debug(RequestTitle);
            _logger.DidNotReceive().Debug(RequestBodyTitle);
            _logger.DidNotReceive().Debug(ResponseTitle);
            _logger.DidNotReceive().Debug(ResponseBodyTitle);
        }

        [Fact]
        public async Task SendAsync_WithRequestAllVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.RequestAll);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(RequestTitle);
            _logger.Received(1).Debug(RequestBodyTitle);
        }

        [Fact]
        public async Task SendAsync_WithRequestHeadersVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.RequestHeaders);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(RequestTitle);
        }

        [Fact]
        public async Task SendAsync_WithRequestBodyVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.RequestBody);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(RequestBodyTitle);
        }

        [Fact]
        public async Task SendAsync_WithResponseAllVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.ResponseAll);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(ResponseTitle);
            _logger.Received(1).Debug(ResponseBodyTitle);
        }

        [Fact]
        public async Task SendAsync_WithResponseHeadersVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.ResponseHeaders);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(ResponseTitle);
        }

        [Fact]
        public async Task SendAsync_WithResponseBodyVerbosity_ExecutesForAll()
        {
            var handler = CreateHandlerWithVerbosity(LogVerbosity.ResponseBody);

            await handler.PublicSendAsync(_request, CancellationToken.None);

            _logger.Received(1).Debug(ResponseBodyTitle);
        }

        private TestHttpDiagnosticsHandler CreateHandlerWithVerbosity(LogVerbosity verbosity)
        {
            return new TestHttpDiagnosticsHandler(_logger, verbosity)
            {
                InnerHandler = _innerHandler
            };
        }
    }
}
