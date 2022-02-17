// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests.Stubs
{
    internal class TaskStub<T>
    {
        private readonly TaskCompletionSource<T> _tcs;

        public TaskStub()
        {
            _tcs = new TaskCompletionSource<T>();
        }

        public Task<T> AsGenericTask => _tcs.Task;

        public Task AsVoidTask => _tcs.Task;

        public bool IsCompleted => _tcs.Task.IsCompleted;

        public void SetResult(T result) => _tcs.SetResult(result);

        public void SetException(Exception exception) => _tcs.SetException(exception);

        public void SetCanceled() => _tcs.SetCanceled();

        public Task AwaitCompletionAsync() => _tcs.Task;

        public async Task AwaitCompletionWithIgnoreExceptionAsync<TException>() where TException : Exception
        {
            try
            {
                await AwaitCompletionAsync();
            }
            catch (TException)
            {
                // ignored
            }
        }
    }
}
