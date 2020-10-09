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
        /// <summary>
        ///     Make a safe remote request with getting result and logging errors.
        ///     Returns <see langword="null"/> when an exception was thrown during execution.
        /// </summary>
        /// <param name="remoteService">Instance of <see cref="IRemoteService{TApiService}"/>.</param>
        /// <param name="operation">
        ///     Delegate that encapsulates request operation (call the method from <see cref="TApiService"/>).
        /// </param>
        /// <param name="cancellationToken">Token for canceling the request.</param>
        /// <param name="logger">Instance of <see cref="ILogger"/> (optional).</param>
        /// <typeparam name="TApiService">The type of API service.</typeparam>
        /// <typeparam name="TResult">The type of request result.</typeparam>
        /// <returns>Task with result.</returns>
        public static async Task<TResult> SafeRequest<TApiService, TResult>(
            this IRemoteService<TApiService> remoteService,
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken,
            ILogger? logger = null)
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
                return default!;
            }
        }

        /// <summary>
        ///     Make a safe remote request without result and with logging errors.
        /// </summary>
        /// <param name="remoteService">Instance of <see cref="IRemoteService{TApiService}"/>.</param>
        /// <param name="operation">
        ///     Delegate that encapsulates request operation (call the method from <see cref="TApiService"/>).
        /// </param>
        /// <param name="cancellationToken">Token for canceling the request.</param>
        /// <param name="logger">Instance of <see cref="ILogger"/> (optional).</param>
        /// <typeparam name="TApiService">Type of API service.</typeparam>
        /// <returns>Task.</returns>
        public static async Task SafeRequest<TApiService>(
            this IRemoteService<TApiService> remoteService,
            Func<TApiService, CancellationToken, Task> operation,
            CancellationToken cancellationToken,
            ILogger? logger = null)
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
