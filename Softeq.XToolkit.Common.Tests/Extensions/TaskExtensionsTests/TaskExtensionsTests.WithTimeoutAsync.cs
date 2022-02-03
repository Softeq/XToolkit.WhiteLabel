// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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

            Assert.ThrowsAsync<ArgumentNullException>(() => timeoutTask);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_TimesOut_ThrowsTimeoutException(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(ShortTimeout)
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
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(LongTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(LongTimeout);

            Assert.True(timeoutTask.IsCompleted);
            await timeoutTask;
        }

        [Fact]
        public async Task WithTimeoutAsync_CompletedGenericTaskAndLongTimeout_TaskCompletesFirstWithResult()
        {
            var expectedResult = "test result";
            _taskStub.SetResult(expectedResult);
            var task = _taskStub.AsGenericTask;

            var result = await task.WithTimeoutAsync<object>(LongTimeout);

            Assert.Same(expectedResult, result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_FailedTaskAndLongTimeout_TaskCompletesFirstAndThrowsException(bool generic)
        {
            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(LongTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(LongTimeout);

            // target task continues to run until exception
            _taskStub.SetException(new CommonTestException());

            await Assert.ThrowsAsync<CommonTestException>(() => timeoutTask);
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
            try
            {
                await timeoutTask;
            }
            catch (TimeoutException)
            {
                // ignored
            }

            // target task continues to run until exception
            _taskStub.SetException(new CommonTestException());

            await Assert.ThrowsAsync<CommonTestException>(() => _taskStub.AwaitCompletionAsync());
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
            try
            {
                await timeoutTask;
            }
            catch (TimeoutException)
            {
                // ignored
            }

            // target task was canceled
            _taskStub.SetCanceled();

            await Assert.ThrowsAsync<TaskCanceledException>(() => _taskStub.AwaitCompletionAsync());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeoutAsync_TimeoutIsInfinite_ExecutesUntilTaskCompletes(bool generic)
        {
            var infiniteTimeout = Timeout.InfiniteTimeSpan;

            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithTimeoutAsync<object>(infiniteTimeout)
                : _taskStub.AsVoidTask.WithTimeoutAsync(infiniteTimeout);

            // target task continues to run until result
            _taskStub.SetResult(null);

            await timeoutTask;

            Assert.True(_taskStub.IsCompleted);
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
