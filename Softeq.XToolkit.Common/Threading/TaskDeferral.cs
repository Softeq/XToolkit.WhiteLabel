// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Threading
{
    /// <summary>
    ///    Execution queue where all tasks get a result of the first task.
    /// </summary>
    /// <typeparam name="T">Type of result.</typeparam>
    public class TaskDeferral<T>
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly ConcurrentQueue<TaskCompletionSource<T>> _queue;

        public TaskDeferral()
        {
            _semaphoreSlim = new SemaphoreSlim(1);
            _queue = new ConcurrentQueue<TaskCompletionSource<T>>();
        }

        /// <summary>
        ///     Adds a task to the execution queue.
        ///     If the queue is not empty, method will wait for the result of task execution from the queue.
        /// </summary>
        /// <param name="taskFactory">Returns Task to be executed.</param>
        /// <returns>Result of task execution.</returns>
        public async Task<T> DoWorkAsync(Func<Task<T>> taskFactory)
        {
            var tcs = new TaskCompletionSource<T>();

            _queue.Enqueue(tcs);

            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            // return result for all awaited tasks
            if (_queue.IsEmpty)
            {
                _semaphoreSlim.Release();
                return await tcs.Task.ConfigureAwait(false);
            }

            var result = await ExecuteAsync(taskFactory).ConfigureAwait(false);

            // set result for all awaited tasks
            while (_queue.TryDequeue(out var item))
            {
                item.SetResult(result);
            }

            _semaphoreSlim.Release();

            return await tcs.Task.ConfigureAwait(false);
        }

        private static async Task<T> ExecuteAsync(Func<Task<T>> taskFactory)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                return default!;
            }
        }
    }
}
