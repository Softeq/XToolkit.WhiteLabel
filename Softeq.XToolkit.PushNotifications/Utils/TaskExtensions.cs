// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Utils
{
    // TEMPORARY COPY
    internal static class TaskExtensions
    {
        public static Task<T> SafeTaskWrapper<T>(this Task<T> task)
        {
            task.ContinueWith(t => { LogException(t); }, CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.Default);
            return task;
        }

        public static Task SafeTaskWrapper(this Task task)
        {
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