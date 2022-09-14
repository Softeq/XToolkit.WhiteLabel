// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client
{
    public class DefaultHttpMessageHandlerBuilderTests
    {
        private readonly HttpMessageHandler _primaryHandler;
        private readonly DefaultHttpMessageHandlerBuilder _httpMessageHandlerBuilder;

        public DefaultHttpMessageHandlerBuilderTests()
        {
            _primaryHandler = Substitute.For<HttpMessageHandler>();

            _httpMessageHandlerBuilder = new DefaultHttpMessageHandlerBuilder(_primaryHandler);
        }

        [Fact]
        public void Ctor_Default_ReturnsExpectedInstance()
        {
            var result = new DefaultHttpMessageHandlerBuilder();

            Assert.NotNull(result.PrimaryHandler);
            Assert.Empty(result.AdditionalHandlers);
        }

        [Fact]
        public void Ctor_NullPrimaryHandler_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultHttpMessageHandlerBuilder(null!));
        }

        [Fact]
        public void Ctor_PrimaryHandler_ReturnsExpectedInstance()
        {
            Assert.Same(_primaryHandler, _httpMessageHandlerBuilder.PrimaryHandler);
            Assert.Empty(_httpMessageHandlerBuilder.AdditionalHandlers);
        }

        [Fact]
        public void AddHandler_NullHandler_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _httpMessageHandlerBuilder.AddHandler(null!);
            });
        }

        [Fact]
        public void AddHandler_CorrectHandler_ExecutesCorrectly()
        {
            var additionalHandler = Substitute.For<DelegatingHandler>();

            var result = _httpMessageHandlerBuilder.AddHandler(additionalHandler);

            Assert.IsAssignableFrom<HttpMessageHandlerBuilder>(result);
            Assert.NotEmpty(_httpMessageHandlerBuilder.AdditionalHandlers);
        }

        [Fact]
        public void AddHandler_TwoAdditionalHandler_AddsCorrectOrder()
        {
            var additionalHandler1 = Substitute.For<DelegatingHandler>();
            var additionalHandler2 = Substitute.For<DelegatingHandler>();

            var result = _httpMessageHandlerBuilder
                .AddHandler(additionalHandler1)
                .AddHandler(additionalHandler2);

            Assert.IsAssignableFrom<HttpMessageHandlerBuilder>(result);
            Assert.Same(additionalHandler1, _httpMessageHandlerBuilder.AdditionalHandlers[0]);
            Assert.Same(additionalHandler2, _httpMessageHandlerBuilder.AdditionalHandlers[1]);
        }

        [Fact]
        public void Build_WithNullPrimaryHandler_ReturnsPrimaryHandler()
        {
            var result = _httpMessageHandlerBuilder.Build();

            Assert.Same(_primaryHandler, result);
        }

        [Fact]
        public void Build_WithOnlyPrimaryHandler_ThrowsArgumentNullException()
        {
            _httpMessageHandlerBuilder.PrimaryHandler = null!;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _httpMessageHandlerBuilder.Build();
            });
        }

        [Fact]
        public void Build_WithPrimaryAndAdditionalHandlers_ReturnsCorrectChain()
        {
            var additionalHandler1 = Substitute.For<DelegatingHandler>();
            var additionalHandler2 = Substitute.For<DelegatingHandler>();
            _httpMessageHandlerBuilder
                .AddHandler(additionalHandler1)
                .AddHandler(additionalHandler2);

            var result = (DelegatingHandler) _httpMessageHandlerBuilder.Build();

            // first handler at the top
            Assert.Same(additionalHandler1, result);

            // second handler at the middle
            Assert.Same(additionalHandler2, result.InnerHandler);

            // primary handler at the root
            Assert.Same(_primaryHandler, ((DelegatingHandler)result.InnerHandler!).InnerHandler);
        }
    }
}
