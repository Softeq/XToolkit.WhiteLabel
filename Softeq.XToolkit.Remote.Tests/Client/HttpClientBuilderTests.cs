// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client
{
    public class HttpClientBuilderTests
    {
        private const string TestBaseUrl = "https://softeq.com/";

        private readonly HttpClientBuilder _clientBuilder;

        public HttpClientBuilderTests()
        {
            var messageHandlerBuilder = Substitute.For<HttpMessageHandlerBuilder>();
            messageHandlerBuilder.Build().Returns(Substitute.For<HttpMessageHandler>());

            _clientBuilder = new HttpClientBuilder(messageHandlerBuilder);
        }

        [Fact]
        public void Ctor_NullMessageHandlerBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpClientBuilder(null!));
        }

        [Fact]
        public void Ctor_CorrectMessageHandlerBuilder_ReturnsCorrectInstance()
        {
            Assert.IsAssignableFrom<IHttpClientBuilder>(_clientBuilder);
        }

        [Theory]
        [InlineData(null!)]
        [InlineData("")]
        public void WithBaseUrl_NullOrEmptyBaseUrl_ThrowsArgumentException(string baseUrl)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new HttpClientBuilder().WithBaseUrl(baseUrl);
            });
        }

        [Fact]
        public void WithBaseUrl_CorrectBaseUrl_ReturnsCorrectInstance()
        {
            var result = new HttpClientBuilder().WithBaseUrl(TestBaseUrl);

            Assert.IsAssignableFrom<IHttpClientBuilder>(result);
        }

        [Fact]
        public void AddHandler_NullHandler_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _clientBuilder.AddHandler(null!);
            });
        }

        [Fact]
        public void AddHandler_CorrectHandler_ReturnsSelf()
        {
            var handler = Substitute.For<DelegatingHandler>();

            var clientBuilder = _clientBuilder.AddHandler(handler);

            Assert.IsAssignableFrom<IHttpClientBuilder>(clientBuilder);
        }

        [Fact]
        public void Build_WithoutConfigurations_ReturnsCorrectHttpClient()
        {
            var result = _clientBuilder.Build();

            Assert.IsAssignableFrom<HttpClient>(result);
        }

        [Fact]
        public void Build_WithBaseUrl_ReturnsCorrectHttpClient()
        {
            var result = _clientBuilder.WithBaseUrl(TestBaseUrl).Build();

            Assert.IsAssignableFrom<HttpClient>(result);
            Assert.Equal(TestBaseUrl, result.BaseAddress?.ToString());
        }
    }
}
