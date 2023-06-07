// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.XToolkit.Remote.Client;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Client
{
    public class HttpMessageHandlerProviderTests
    {
        [Fact]
        public void CreateDefaultHandler_Always_ReturnsHttpMessageHandler()
        {
            var result = HttpMessageHandlerProvider.CreateOrGetCachedDefaultHandler();

            Assert.IsAssignableFrom<HttpMessageHandler>(result);
        }

        [Fact]
        public void CreateOrGetCachedDefaultHandler_Always_ReturnsHttpMessageHandler()
        {
            var result = HttpMessageHandlerProvider.CreateDefaultHandler();

            Assert.IsAssignableFrom<HttpMessageHandler>(result);
        }

        [Fact]
        public void CreateOrGetCachedDefaultHandler_Always_ReturnsSameHandler()
        {
            var firstHandler = HttpMessageHandlerProvider.CreateOrGetCachedDefaultHandler();
            var secondHandler = HttpMessageHandlerProvider.CreateOrGetCachedDefaultHandler();

            Assert.Equal(firstHandler, secondHandler);
        }

        [Fact]
        public void CreateDefaultHandler_Always_ReturnsDifferentsHandler()
        {
            var firstHandler = HttpMessageHandlerProvider.CreateDefaultHandler();
            var secondHandler = HttpMessageHandlerProvider.CreateDefaultHandler();

            Assert.NotEqual(firstHandler, secondHandler);
        }
    }
}
