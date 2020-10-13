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
            var result = HttpMessageHandlerProvider.CreateDefaultHandler();

            Assert.IsAssignableFrom<HttpMessageHandler>(result);
        }
    }
}
