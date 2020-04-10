// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Xunit;
using TaskExt = Softeq.XToolkit.Common.Extensions.TaskExtensions;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod", Justification = "Need for test")]
    public class TaskExtensionsTests
    {
        private const int AsyncDelay = 500;

        private readonly ILogger _logger;
        private readonly TaskCompletionSource<object> _tcs;
        private readonly Action<Exception> _onException;

        public TaskExtensionsTests()
        {
            _logger = Substitute.For<ILogger>();
            _tcs = new TaskCompletionSource<object>();
            _onException = Substitute.For<Action<Exception>>();
        }

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
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(-1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(-1));

            Assert.False(timeoutTask.IsCompleted);

            await Task.Delay(AsyncDelay / 2);

            Assert.False(timeoutTask.IsCompleted);

            _tcs.SetResult(null);

            timeoutTask.GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_TimesOut(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync(TimeSpan.FromMilliseconds(1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.Throws<TimeoutException>(() => timeoutTask.GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_CompletesFirst(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            _tcs.SetResult(null);

            timeoutTask.GetAwaiter().GetResult();
        }

        [Fact]
        public void WithTimeout_CompletesFirstWithResult()
        {
            var timeoutTask = _tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            _tcs.SetResult("success");

            Assert.Same(_tcs.Task.Result, timeoutTask.Result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_CompletesFirstAndThrows(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync(TimeSpan.FromDays(1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            _tcs.SetException(new ApplicationException());

            await Assert.ThrowsAsync<ApplicationException>(() => timeoutTask);
            Assert.Same(_tcs.Task.Exception.InnerException, timeoutTask.Exception.InnerException);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalThrows(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            _tcs.SetException(new ApplicationException());

            Assert.Throws<ApplicationException>(() => _tcs.Task.GetAwaiter().GetResult());
            Assert.True(_tcs.Task.IsFaulted);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalCancel(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task.WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) _tcs.Task).WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            _tcs.SetCanceled();

            Assert.Throws<TaskCanceledException>(() => _tcs.Task.GetAwaiter().GetResult());
            Assert.True(_tcs.Task.IsCanceled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_TimesOutWithInternalThrowsLog(bool generic)
        {
            var timeoutTask = generic
                ? _tcs.Task
                    .WithLoggingErrors(_logger)
                    .WithTimeoutAsync<object>(TimeSpan.FromMilliseconds(1))
                : ((Task) _tcs.Task)
                    .WithLoggingErrors(_logger)
                    .WithTimeoutAsync(TimeSpan.FromMilliseconds(1));

            Assert.False(timeoutTask.IsCompleted);

            await Assert.ThrowsAsync<TimeoutException>(() => timeoutTask);

            _tcs.SetException(new ApplicationException());

            await Task.Delay(10);

            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void LogWrapper_NullLogger(bool generic)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = generic
                    ? _tcs.Task.WithLoggingErrors<object>(null)
                    : _tcs.Task.WithLoggingErrors(null);
            });
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Executes(bool generic)
        {
            _tcs.SetResult(null);

            var wrappedTask = generic
                ? _tcs.Task.WithLoggingErrors<object>(_logger)
                : _tcs.Task.WithLoggingErrors(_logger);

            await wrappedTask;

            await _tcs.Task;

            _logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Throws(bool generic)
        {
            _tcs.SetException(new InvalidOperationException());

            var wrappedTask = generic
                ? _tcs.Task.WithLoggingErrors<object>(_logger)
                : _tcs.Task.WithLoggingErrors(_logger);

            await Assert.ThrowsAsync<InvalidOperationException>(() => wrappedTask);

            await Task.Delay(10);

            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LogWrapper_Cancels(bool generic)
        {
            _tcs.SetCanceled();

            var wrappedTask = generic
                ? _tcs.Task.WithLoggingErrors<object>(_logger)
                : _tcs.Task.WithLoggingErrors(_logger);

            await Assert.ThrowsAsync<TaskCanceledException>(() => wrappedTask);

            await Task.Delay(10);

            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_ExecutesWithoutException()
        {
            _tcs.SetException(new ApplicationException());

            _tcs.Task.FireAndForget();

            await Task.Delay(10);
        }

        [Fact]
        public async Task FireAndForget_LoggerNull_Executes()
        {
            _tcs.SetResult(null);

            _tcs.Task.FireAndForget(null as ILogger);

            await _tcs.Task;
        }

        [Fact]
        public async Task FireAndForget_Executes()
        {
            _tcs.SetResult(null);

            _tcs.Task.FireAndForget(_logger);

            await _tcs.Task;

            _logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_ExecutesAndLogs()
        {
            _tcs.SetException(new ApplicationException());

            _tcs.Task.FireAndForget(_logger);

            await Task.Delay(10);

            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_CancelledAndLogs()
        {
            _tcs.SetCanceled();

            _tcs.Task.FireAndForget(_logger);

            await Assert.ThrowsAsync<TaskCanceledException>(() => _tcs.Task);

            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_OnExceptionWithResult()
        {
            _tcs.SetResult(null);

            _tcs.Task.FireAndForget(_onException);

            await _tcs.Task;

            _onException.DidNotReceive().Invoke(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_OnExceptionNull()
        {
            _tcs.SetResult(null);

            _tcs.Task.FireAndForget(null as Action<Exception>);

            await _tcs.Task;
        }

        [Fact]
        public async Task FireAndForget_OnExceptionWithException()
        {
            _tcs.SetCanceled();

            _tcs.Task.FireAndForget(_onException);

            await Task.Delay(10);

            _onException.Received(1).Invoke(Arg.Any<TaskCanceledException>());
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
