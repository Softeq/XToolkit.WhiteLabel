// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    /// <summary>
    ///     An abstraction for a component that can make requests with options.
    /// </summary>
    /// <typeparam name="TApiService">Type of API service.</typeparam>
    public interface IRemoteService<out TApiService>
    {
        /// <summary>
        ///     Make a simple remote request without result.
        /// </summary>
        /// <param name="operation">
        ///     Delegate that encapsulates request operation (call the method from <see cref="TApiService"/>).
        /// </param>
        /// <param name="options">Request options (optional).</param>
        /// <returns>Task.</returns>
        Task MakeRequest(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions? options = null);

        /// <summary>
        ///     Make a remote request with getting result.
        /// </summary>
        /// <param name="operation">
        ///    Delegate that encapsulates request operation (call the method from <see cref="TApiService"/>).
        /// </param>
        /// <param name="options">Request options (optional).</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns>Task with result.</returns>
        Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions? options = null);
    }
}
