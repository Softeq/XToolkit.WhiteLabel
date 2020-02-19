// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public static class RemoteServiceExtensions
    {
        public static async Task<TResult> SafeRequest<TApiService, TResult>(
            this IRemoteService<TApiService> remoteService,
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken,
            ILogger logger = null)
        {
            try
            {
                return await remoteService.MakeRequest(
                    operation,
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });
            }
            catch (Exception ex)
            {
                logger?.Error(ex);
                return default;
            }
        }

        public static async Task SafeRequest<TApiService>(
            this IRemoteService<TApiService> remoteService,
            Func<TApiService, CancellationToken, Task> operation,
            CancellationToken cancellationToken,
            ILogger logger = null)
        {
            try
            {
                await remoteService.MakeRequest(
                    operation,
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });
            }
            catch (Exception ex)
            {
                logger?.Error(ex);
            }
        }
    }
}
