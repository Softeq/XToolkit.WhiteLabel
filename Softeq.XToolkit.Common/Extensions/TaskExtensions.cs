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
        ///     Returns a task that completes as the original task completes or when a timeout expires, whichever happens first.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The maximum time to wait.</param>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/> or
        ///     faults with a <see cref="TimeoutException"/> if <paramref name="timeout"/> elapses first.
        /// </returns>
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
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

                // The timeout did not elapse, so cancel the timer to recover system resources.
                timerCancellation.Cancel();

                // re-throw any exceptions from the completed task.
                await task.ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Returns a task that completes as the original task completes or when a timeout expires, whichever happens first.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the original task.</typeparam>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The maximum time to wait.</param>
        /// <returns>
        ///     A task that completes with the result of the specified <paramref name="task"/> or
        ///     faults with a <see cref="TimeoutException"/> if <paramref name="timeout"/> elapses first.
        /// </returns>
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            await WithTimeout((Task) task, timeout).ConfigureAwait(false);
            return task.GetAwaiter().GetResult();
        }

        public static Task<T> SafeTaskWrapper<T>(this Task<T> task, ILogger logger = null)
        {
            task.ContinueWith(t => { LogException(t, logger); }, CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);
            return task;
        }

        public static Task SafeTaskWrapper(this Task task, ILogger logger = null)
        {
            //todo PL: implement default logger
            task.ContinueWith(t => { LogException(t, logger); }, CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);
            return task;
        }

        private static void LogException(Task task, ILogger logger)
        {
            Exception exception = task.Exception;
            if (exception != null && logger != null)
            {
                logger.Error(exception.StackTrace);
                if (exception.InnerException != null)
                {
                    logger.Error(exception.InnerException.StackTrace);
                }
            }
        }
    }
}
