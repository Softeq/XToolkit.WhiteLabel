// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Auth.Handlers
{
    /// <summary>
    ///    The message handler to support the Authorization header that adds access token if needed.
    /// </summary>
    /// <remarks>
    ///     Migrated Refit private implementation:
    ///     https://github.com/reactiveui/refit/blob/2cc549bf06f84e8eddb6e3dcb67bebf81ce5f642/Refit/AuthenticatedHttpClientHandler.cs .
    /// </remarks>
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly Func<Task<string>> _getToken;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticatedHttpClientHandler"/> class.
        /// </summary>
        /// <param name="getToken">Access token provider.</param>
        public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
        {
            _getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                var token = await _getToken().ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
