// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Utils
{
    // TEMPORARY COPY
    public static class TaskExtensions
    {
        /// <summary>
        ///     Task extension to add a timeout.
        /// </summary>
        /// <returns>The task with timeout.</returns>
        /// <param name="task">Task.</param>
        /// <param name="timeout"></param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            var retTask = await Task.WhenAny(task, Task.Delay((int) timeout.TotalMilliseconds))
                .ConfigureAwait(false);

            if (retTask is Task<T>)
            {
                return task.Result;
            }

            return default(T);
        }

        public static Task<T> SafeTaskWrapper<T>(this Task<T> task)
        {
            task.ContinueWith(t => { LogException(t); }, CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);
            return task;
        }

        public static Task SafeTaskWrapper(this Task task)
        {
            //todo PL: implement default logger
            task.ContinueWith(t => { LogException(t); }, CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);
            return task;
        }

        private static void LogException(Task task)
        {
            Exception exception = task.Exception;
            if (exception != null)
            {
                Console.WriteLine(exception.StackTrace);
                if (exception.InnerException != null)
                {
                    Console.WriteLine(exception.InnerException.StackTrace);
                }
            }
        }
    }
}