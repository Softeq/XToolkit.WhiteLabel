// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Auth.Handlers;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests
{
    internal class TestRefreshTokenHttpClientHandler : RefreshTokenHttpClientHandler
    {
        public TestRefreshTokenHttpClientHandler(
            Func<Task<string>> getToken,
            Func<Task> getRefreshedToken)
            : base(getToken, getRefreshedToken)
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
