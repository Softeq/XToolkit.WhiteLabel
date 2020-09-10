// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.Threading.Tasks.Task"/>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///     <see cref="T:System.Threading.Tasks.Task"/> extension to add a timeout.
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

            return default!;
        }

        /// <summary>
        ///     Returns a task that completes as the original task completes or when a timeout expires, whichever happens first.
        ///     For logging exceptions of <paramref name="task"/> use <see cref="WithLoggingErrors"/> method before calling this.
        /// </summary>
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
        ///     For logging exceptions of <paramref name="task"/> use <see cref="WithLoggingErrors{T}"/> method before calling this.
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
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        ///     Attaches the <paramref name="logger"/> to the <paramref name="task"/> for logging exceptions.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="logger">Logger implementation.</param>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">When <paramref name="logger"/> is null.</exception>
        public static Task WithLoggingErrors(this Task task, ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            task.ContinueWith(
                notSuccessfullyCompletedTask =>
                {
                    if (notSuccessfullyCompletedTask.IsCanceled && notSuccessfullyCompletedTask.Exception == null)
                    {
                        LogException(new TaskCanceledException(notSuccessfullyCompletedTask), logger);
                    }
                    else
                    {
                        LogException(notSuccessfullyCompletedTask.Exception, logger);
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion,
                TaskScheduler.Default);

            return task;
        }

        /// <summary>
        ///     Attaches the <paramref name="logger"/> to the <paramref name="task"/> for logging exceptions.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the original task.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="logger">Logger implementation.</param>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">When <paramref name="logger"/> is null.</exception>
        public static Task<T> WithLoggingErrors<T>(this Task<T> task, ILogger logger)
        {
            WithLoggingErrors((Task) task, logger);
            return task;
        }

        /// <summary>
        ///     Simple wrapper for execution task with <see cref="CancellationToken"/>.
        ///     Target Task will continue running, but the execution will be returned.
        /// </summary>
        /// <param name="task">The task to cancellation for.</param>
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken"/> that will be assigned to the new continuation task.
        /// </param>
        /// <typeparam name="T">Type of Task.</typeparam>
        /// <exception cref="TaskCanceledException">The task was canceled.</exception>
        /// <returns>Task result.</returns>
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
        ///     Exceptions will be ignored.
        /// </summary>
        /// <param name="task">The task.</param>
        public static void FireAndForget(this Task task)
        {
            FireAndForget(task, _ => { });
        }

        /// <summary>
        ///     Useful for fire-and-forget calls to async methods.
        ///     Exceptions will be written to the <paramref name="logger"/>.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="logger">Logger implementation.</param>
        public static void FireAndForget(this Task task, ILogger? logger)
        {
            FireAndForget(task, ex => LogException(ex, logger));
        }

        /// <summary>
        ///     Useful for fire-and-forget calls to async methods.
        ///     Exceptions will get to the callback <paramref name="onException"/>.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="onException">
        ///     If an exception is thrown in the Task, <c>onException</c> will execute.
        ///     If onException is null, the exception will be re-thrown.
        /// </param>
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForget(this Task task, Action<Exception> onException)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                onException?.Invoke(exception);
            }
        }

        private static void LogException(Exception exception, ILogger? logger)
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
