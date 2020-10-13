// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth.Handlers;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.AuthenticatedHttpClientHandlerTests
{
    internal class TestAuthenticatedHttpClientHandler : AuthenticatedHttpClientHandler
    {
        public TestAuthenticatedHttpClientHandler(
            Func<Task<string>> getToken)
            : base(getToken)
        {
        }

        public Task<HttpResponseMessage> PublicSendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken);
        }
    }
}
