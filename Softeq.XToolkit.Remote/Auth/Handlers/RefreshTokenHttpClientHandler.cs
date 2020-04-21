// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
﻿using System.Net;
﻿using System.Net.Http;
﻿using System.Threading;
﻿using System.Threading.Tasks;
﻿using Polly;
﻿using Softeq.XToolkit.Remote.Exceptions;

﻿namespace Softeq.XToolkit.Remote.Auth.Handlers
{
    public class RefreshTokenHttpClientHandler : AuthenticatedHttpClientHandler
    {
        private readonly Func<Task> _getRefreshedToken;

        public RefreshTokenHttpClientHandler(
            Func<Task<string>> getAccessToken,
            Func<Task> getRefreshedToken)
            : base(getAccessToken)
        {
            _getRefreshedToken = getRefreshedToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            const int RetryStatesCount = 2;

            return await Policy
                .HandleResult<HttpResponseMessage>(response => response.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(RetryStatesCount, OnRetryAsync)
                .ExecuteAsync(async ct => await base.SendAsync(request, ct).ConfigureAwait(false), cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task OnRetryAsync(DelegateResult<HttpResponseMessage> result, int attempt)
        {
            const int AccessTokenExpired = 1;
            const int RefreshTokenExpired = 2;

            switch (attempt)
            {
                case AccessTokenExpired:
                    await _getRefreshedToken().ConfigureAwait(false);
                    break;
                case RefreshTokenExpired:
                    throw new ExpiredRefreshTokenException(result.Exception);
                default:
                    throw new InvalidOperationException($"Can't handle attempt number: {attempt.ToString()}", result.Exception);
            }
        }
    }
}
