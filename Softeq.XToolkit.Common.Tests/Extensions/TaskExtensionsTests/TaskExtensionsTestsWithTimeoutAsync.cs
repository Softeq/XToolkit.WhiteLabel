// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Xunit;
using TaskExt = Softeq.XToolkit.Common.Extensions.TaskExtensions;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod", Justification = "Need for test")]
    public partial class TaskExtensionsTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeoutAsync_WhenTaskIsNull_ThrowsArgumentNullException(bool generic)
        {
            var timeoutTask = generic
                ? TaskExt.WithTimeoutAsync<int>(null!, ShortTimeout)
                : TaskExt.WithTimeoutAsync(null!, ShortTimeout);

            Assert.Throws<ArgumentNullException>(() =>
            {
                timeoutTask.GetAwaiter().GetResult();
            });
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_TimesOut_ThrowsTimeoutException(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync(ShortTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(ShortTimeout);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_CompletedTaskAndLongTimeout_TaskCompletesFirst(bool generic)
        {
            _taskStub.SetResult(null);

            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync(LongTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(LongTimeout);

            Assert.True(timeoutTask.IsCompleted);
            await timeoutTask;
        }

        [Fact]
        public void WithTimeoutAsync_CompletedGenericTaskAndLongTimeout_TaskCompletesFirstWithResult()
        {
            _taskStub.SetResult("success");
            var task = _taskStub.AsGenericTask;

            var timeoutTask = task.WithTimeoutAsync(LongTimeout);

            Assert.Same(task.Result, timeoutTask.Result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_FailedTaskAndLongTimeout_TaskCompletesFirstAndThrowsInnerException(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync(LongTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(LongTimeout);

            // target task continues to run until exception
            _taskStub.SetException(new CommonTestException());

            await Assert.ThrowsAsync<CommonTestException>(() => timeoutTask);
            Assert.Same(_taskStub.InnerException, timeoutTask.Exception!.InnerException);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_FailedTaskAndTimeout_TimesOutAndContinueExecutionUntilException(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(ShortTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(ShortTimeout);

            // result task finished by timeout
            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            // target task continues to run until exception
            _taskStub.SetException(new CommonTestException());

            await Assert.ThrowsAsync<CommonTestException>(() => _taskStub.AwaitResultAsync());
            Assert.True(_taskStub.IsFaulted);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_CanceledTaskAndTimeout_TimesOutAndCancellation(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(ShortTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(ShortTimeout);

            // result task finished by timeout
            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            // target task was canceled
            _taskStub.SetCanceled();

            await Assert.ThrowsAsync<TaskCanceledException>(() => _taskStub.AwaitResultAsync());
            Assert.True(_taskStub.IsCanceled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_TimeoutIsInfinite_ExecutesUntilTaskCompletes(bool generic)
        {
            var infiniteTimeout = TimeSpan.FromMilliseconds(-1);

            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(infiniteTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(infiniteTimeout);

            Assert.False(timeoutTask.IsCompleted);

            _taskStub.SetResult(null);

            await timeoutTask;

            Assert.True(timeoutTask.IsCompleted);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_TimeoutIsNegative_ThrowsArgumentOutOfRangeException(bool generic)
        {
            var negativeTimeout = TimeSpan.FromMilliseconds(-2);

            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(negativeTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(negativeTimeout);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => timeoutTask);
        }
    }
}
