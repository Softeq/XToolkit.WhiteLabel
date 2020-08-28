// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Tasks;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Tasks
{
    public class TaskDeferralTests
    {
        private readonly TaskDeferral<string> _deferral;

        public TaskDeferralTests()
        {
            _deferral = new TaskDeferral<string>();
        }

        [Fact]
        public async Task DoWorkAsync_Null_ReturnsNull()
        {
            var result = await _deferral.DoWorkAsync(null!);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoWorkAsync_OneTaskWithResult_ReturnsResult()
        {
            var funcTask = CreateTaskWithResult("test", 10);

            var result = await _deferral.DoWorkAsync(funcTask);

            Assert.Equal("test", result);
        }

        [Fact]
        public async Task DoWorkAsync_ThreeTasksWithResult_EveryTaskGetsResultOfFirstTask()
        {
            var task1Factory = CreateTaskWithResult("test1", 50);
            var task2Factory = CreateTaskWithResult("test2", 10);
            var task3Factory = CreateTaskWithResult("test3", 20);

            var task1 = _deferral.DoWorkAsync(task1Factory);
            var task2 = _deferral.DoWorkAsync(task2Factory);
            var task3 = _deferral.DoWorkAsync(task3Factory);

            var results = await Task.WhenAll(task1, task2, task3);

            Assert.All(results, result => Assert.Equal("test1", result));
        }

        [Fact]
        public async Task DoWorkAsync_TwoTasksWithFirstWithException_EveryTaskGetsNull()
        {
            var task1Factory = CreateTaskWithException(10);
            var task2Factory = CreateTaskWithResult("test2", 20);

            var task1 = _deferral.DoWorkAsync(task1Factory);
            var task2 = _deferral.DoWorkAsync(task2Factory);

            var results = await Task.WhenAll(task1, task2);

            Assert.All(results, Assert.Null);
        }

        [Fact]
        public async Task DoWorkAsync_TwoTasksWithFirstWithCancellation_EveryTaskGetsNull()
        {
            var task1Factory = CreateCancelledTask(10);
            var task2Factory = CreateTaskWithResult("test2", 20);

            var task1 = _deferral.DoWorkAsync(task1Factory);
            var task2 = _deferral.DoWorkAsync(task2Factory);

            var results = await Task.WhenAll(task1, task2);

            Assert.All(results, Assert.Null);
        }

        [Fact]
        public async Task DoWorkAsync_TwoTasksWithSecondWithException_EveryTaskGetsResultOfFirstTask()
        {
            var task1Factory = CreateTaskWithResult("test1", 20);
            var task2Factory = CreateTaskWithException(10);

            var task1 = _deferral.DoWorkAsync(task1Factory);
            var task2 = _deferral.DoWorkAsync(task2Factory);

            var results = await Task.WhenAll(task1, task2);

            Assert.All(results, result => Assert.Equal("test1", result));
        }

        private Func<Task<T>> CreateTaskWithResult<T>(T result, int delay)
        {
            return async () =>
            {
                await Task.Delay(delay);
                return result;
            };
        }

        private Func<Task<string>> CreateTaskWithException(int delay)
        {
            return async () =>
            {
                await Task.Delay(delay);
                throw new Exception("test exception");
            };
        }

        private Func<Task<string>> CreateCancelledTask(int delay)
        {
            return async () =>
            {
                var tcs = new TaskCompletionSource<string>();
                await Task.Delay(delay);
                tcs.SetCanceled();
                return await tcs.Task;
            };
        }
    }
}
