// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Xunit;
using TaskExt = Softeq.XToolkit.Common.Extensions.TaskExtensions;

namespace Softeq.XToolkit.Common.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        private const int AsyncDelay = 500;

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_NullTask(bool generic)
        {
            var timeoutTask = generic
                ? TaskExt.WithTimeoutAsync<int>(null, TimeSpan.FromSeconds(1))
                : TaskExt.WithTimeoutAsync(null, TimeSpan.FromSeconds(1));

            Assert.Throws<ArgumentNullException>(() => timeoutTask.GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_MinusOneMeansInfiniteTimeout(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(-1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(-1));

            Assert.False(timeoutTask.IsCompleted);

            await Task.Delay(AsyncDelay / 2);

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetResult(null);

            timeoutTask.GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_TimesOut(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync(TimeSpan.FromMilliseconds(1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.Throws<TimeoutException>(() => timeoutTask.GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_CompletesFirst(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetResult(null);

            timeoutTask.GetAwaiter().GetResult();
        }

        [Fact]
        public void WithTimeout_CompletesFirstWithResult()
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetResult("success");

            Assert.Same(tcs.Task.Result, timeoutTask.Result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_CompletesFirstAndThrows(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetException(new ApplicationException());

            await Assert.ThrowsAsync<ApplicationException>(() => timeoutTask);
            Assert.Same(tcs.Task.Exception.InnerException, timeoutTask.Exception.InnerException);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalThrows(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();

            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            tcs.SetException(new ApplicationException());

            Assert.Throws<ApplicationException>(() => tcs.Task.GetAwaiter().GetResult());
            Assert.True(tcs.Task.IsFaulted);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalCancel(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();

            var timeoutTask = generic
                ? tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            tcs.SetCanceled();

            Assert.Throws<TaskCanceledException>(() => tcs.Task.GetAwaiter().GetResult());
            Assert.True(tcs.Task.IsCanceled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalThrowsLog(bool generic)
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            var timeoutTask = generic
                ? tcs.Task
                    .WithLoggingErrors(logger)
                    .WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) tcs.Task)
                    .WithLoggingErrors(logger)
                    .WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            tcs.SetException(new ApplicationException());

            await Task.Delay(1);

            logger.Received().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void LogWrapper_NullLogger(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = generic
                    ? tcs.Task.WithLoggingErrors<object>(null)
                    : tcs.Task.WithLoggingErrors(null);
            });
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Executes(bool generic)
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetResult(null);

            var wrappedTask = generic
                ? tcs.Task.WithLoggingErrors<object>(logger)
                : tcs.Task.WithLoggingErrors(logger);

            await wrappedTask;

            await Task.Delay(1);

            logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Throws(bool generic)
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetException(new ApplicationException());

            var wrappedTask = generic
                ? tcs.Task.WithLoggingErrors<object>(logger)
                : tcs.Task.WithLoggingErrors(logger);

            await Assert.ThrowsAsync<ApplicationException>(() => wrappedTask);

            await Task.Delay(1);

            logger.Received().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Cancels(bool generic)
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetCanceled();

            var wrappedTask = generic
                ? tcs.Task.WithLoggingErrors<object>(logger)
                : tcs.Task.WithLoggingErrors(logger);

            await Assert.ThrowsAsync<TaskCanceledException>(() => wrappedTask);

            await Task.Delay(1);

            logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_Executes()
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetResult(null);

            tcs.Task.FireAndForget(logger);

            await Task.Delay(1);

            logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_ExecutesAndLogs()
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetException(new ApplicationException());

            tcs.Task.FireAndForget(logger);

            await Task.Delay(1);

            logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_CancelledAndLogs()
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();

            tcs.SetCanceled();

            tcs.Task.FireAndForget(logger);

            await Assert.ThrowsAsync<TaskCanceledException>(() => tcs.Task);

            await Task.Delay(1);

            logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task WithCancellation_Executes()
        {
            var cts = new CancellationTokenSource();
            var tcs = new TaskCompletionSource<int>();

            var t = tcs.Task.WithCancellation(cts.Token);

            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => t);
        }
    }
}
