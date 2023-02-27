// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "More readable")]
    public class DefaultHttpClientFactoryTests
    {
        private const string TestBaseUrl = "https://softeq.com/";

        private readonly ILogger _logger;
        private readonly ISessionContext _sessionContext;
        private readonly DefaultHttpClientFactory _factory;

        public DefaultHttpClientFactoryTests()
        {
            _logger = Substitute.For<ILogger>();
            _sessionContext = Substitute.For<ISessionContext>();
            _factory = new DefaultHttpClientFactory();
        }

        [Fact]
        public void CreateClient_NullBaseUrl_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateClient(null!, _logger);
            });
        }

        [Fact]
        public void CreateClient_EmptyBaseUrl_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateClient(string.Empty, _logger);
            });
        }

        [Fact]
        public void CreateClient_CorrectBaseUrl_ReturnsCorrectClient()
        {
            var result = _factory.CreateClient(TestBaseUrl, _logger);

            Assert.IsAssignableFrom<HttpClient>(result);
            Assert.Equal(TestBaseUrl, result.BaseAddress?.ToString());
        }

        [Fact]
        public void CreateClient_NullLogger_ReturnsClient()
        {
            var result = _factory.CreateClient(TestBaseUrl, null);

            Assert.IsAssignableFrom<HttpClient>(result);
        }

        [Fact]
        public void CreateAuthClient_NullBaseUrl_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateAuthClient(null!, _sessionContext, _logger);
            });
        }

        [Fact]
        public void CreateAuthClient_EmptyBaseUrl_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.CreateAuthClient(string.Empty, _sessionContext, _logger);
            });
        }

        [Fact]
        public void CreateAuthClient_CorrectBaseUrl_ReturnsCorrectClient()
        {
            var result = _factory.CreateAuthClient(TestBaseUrl, _sessionContext, _logger);

            Assert.IsAssignableFrom<HttpClient>(result);
            Assert.Equal(TestBaseUrl, result.BaseAddress?.ToString());
        }

        [Fact]
        public void CreateAuthClient_NullSessionContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.CreateAuthClient(TestBaseUrl, null!, _logger);
            });
        }

        [Fact]
        public void CreateAuthClient_NullLogger_ReturnsClient()
        {
            var result = _factory.CreateAuthClient(TestBaseUrl, _sessionContext, null);

            Assert.IsAssignableFrom<HttpClient>(result);
        }
    }
}
