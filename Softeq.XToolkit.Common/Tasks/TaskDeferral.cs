// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Tasks
{
    public class TaskDeferral<T>
    {
        private readonly ConcurrentQueue<TaskCompletionSource<T>> _queue;
        private readonly SemaphoreSlim _semaphoreSlim;

        public TaskDeferral()
        {
            _semaphoreSlim = new SemaphoreSlim(1);
            _queue = new ConcurrentQueue<TaskCompletionSource<T>>();
        }

        public async Task<T> DoWorkAsync(Func<Task<T>> func)
        {
            var taskReference = new TaskReference<T>(func);

            var tcs = new TaskCompletionSource<T>();

            _queue.Enqueue(tcs);

            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            if (_queue.Count == 0)
            {
                _semaphoreSlim.Release();
                return await tcs.Task;
            }

            T result;

            try
            {
                result = await taskReference.RunAsync().ConfigureAwait(false);
            }
            catch
            {
                result = default;
            }

            while (_queue.Count != 0)
            {
                if (_queue.TryDequeue(out var item))
                {
                    item.SetResult(result);
                }
            }

            _semaphoreSlim.Release();

            return await tcs.Task;
        }
    }
}
