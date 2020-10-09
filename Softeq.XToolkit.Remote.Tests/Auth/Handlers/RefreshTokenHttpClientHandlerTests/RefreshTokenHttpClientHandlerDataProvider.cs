// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests
{
    internal static class RefreshTokenHttpClientHandlerDataProvider
    {
        public static CancellationToken CreateCancellationToken()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            return cts.Token;
        }

        public static Task<HttpResponseMessage> CreateUnauthorizedMessage()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
            return Task.FromResult(response);
        }

        public static Task<HttpResponseMessage> CreateMessageWithInternalException()
        {
            return Task.FromException<HttpResponseMessage>(new Exception());
        }
    }
}
