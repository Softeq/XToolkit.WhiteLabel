// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        ///     Task extension to add a timeout.
        /// </summary>
        /// <returns>The task with timeout.</returns>
        /// <param name="task">Task.</param>
        /// <param name="timeout">Timeout.</param>
        /// <typeparam name="T">Task type.</typeparam>
        [Obsolete("Use WithTimeoutAsync<T> method instead.")]
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            var retTask = await Task.WhenAny(task, Task.Delay((int) timeout.TotalMilliseconds)).ConfigureAwait(false);

            if (retTask is Task<T>)
            {
                return task.Result;
            }

            return default;
        }

        /// <summary>
        ///     Returns a task that completes as the original task completes or when a timeout expires, whichever happens first.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The maximum time to wait.</param>
        /// <exception cref="TimeoutException">When time is over.</exception>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/> or
        ///     faults with a <see cref="TimeoutException"/> if <paramref name="timeout"/> elapses first.
        /// </returns>
        public static async Task WithTimeoutAsync(this Task task, TimeSpan timeout)
        {
            // Details:
            //     https://devblogs.microsoft.com/pfxteam/crafting-a-task-timeoutafter-method/
            // Source:
            //     https://github.com/microsoft/vs-threading/blob/master/src/Microsoft.VisualStudio.Threading/TplExtensions.cs#L73

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            using (var timerCancellation = new CancellationTokenSource())
            {
                var timeoutTask = Task.Delay(timeout, timerCancellation.Token);
                var firstCompletedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

                if (firstCompletedTask == timeoutTask)
                {
                    throw new TimeoutException();
                }

                timerCancellation.Cancel();

                await task.ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Returns a task that completes as the original task completes or when a timeout expires, whichever happens first.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the original task.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The maximum time to wait.</param>
        /// <exception cref="TimeoutException">When time is over.</exception>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/> or
        ///     faults with a <see cref="TimeoutException"/> if <paramref name="timeout"/> elapses first.
        /// </returns>
        /// <example>
        ///     If you need timeout with cancellation, use the better approach with <see cref="CancellationToken"/>:
        /// <code>
        ///     using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        ///     {
        ///         result = await tc.WithCancellation(cts.Token);
        ///     }
        /// </code>
        /// </example>
        public static async Task<T> WithTimeoutAsync<T>(this Task<T> task, TimeSpan timeout)
        {
            await WithTimeoutAsync((Task) task, timeout).ConfigureAwait(false);
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Simple wrapper for execution task with <see cref="CancellationToken"/>.
        ///     Target Task will continue running, but the execution will be returned.
        /// </summary>
        /// <param name="task">The task to cancellation for.</param>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken"/> that will be assigned to the new continuation task.
        /// </param>
        /// <exception cref="TaskCanceledException">The task was canceled.</exception>
        /// <returns></returns>
        public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            return task.IsCompleted
                ? task
                : task.ContinueWith(
                    completedTask => completedTask.GetAwaiter().GetResult(),
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }

        /// <summary>
        ///     Useful for fire-and-forget calls to async methods.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="logger">Logger implementation.</param>
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForget(this Task task, ILogger logger = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                LogException(exception, logger);
            }
        }

        private static void LogException(Exception exception, ILogger logger)
        {
            if (exception == null || logger == null)
            {
                return;
            }

            logger.Error(exception);

            if (exception.InnerException != null)
            {
                logger.Error(exception.InnerException);
            }
        }
    }
}
