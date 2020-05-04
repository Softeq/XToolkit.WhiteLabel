// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System.Threading.Tasks;
﻿using Softeq.XToolkit.Remote.Auth.Handlers;
﻿using Softeq.XToolkit.Remote.Client;

﻿namespace Softeq.XToolkit.Remote.Auth
{
    public static class Extensions
    {
        public static IHttpClientBuilder WithSessionContext(
            this IHttpClientBuilder httpClientBuilder,
            ISessionContext sessionContext)
        {
            var handler = new RefreshTokenHttpClientHandler(
                () => Task.FromResult(sessionContext.AccessToken),
                sessionContext.RefreshTokenAsync);

            return httpClientBuilder.AddHandler(handler);
        }
    }
}
