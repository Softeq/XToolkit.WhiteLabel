// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests.Stubs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod", Justification = "Need for test")]
    public partial class TaskExtensionsTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithLoggingErrors_WhenLoggerIsNull_ThrowsArgumentNullException(bool generic)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                if (generic)
                {
                    _taskStub.AsGenericTask.WithLoggingErrors<object>(null!);
                }
                else
                {
                    _taskStub.AsVoidTask.WithLoggingErrors(null!);
                }
            });
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithLoggingErrors_CompletedTaskWithLogger_ExecutesWithoutAnyLog(bool generic)
        {
            _taskStub.SetResult(null);

            var wrappedTask = generic
                ? _taskStub.AsGenericTask.WithLoggingErrors<object>(_logger)
                : _taskStub.AsVoidTask.WithLoggingErrors(_logger);

            await wrappedTask;

            _logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Theory(Timeout = 10000)]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithLoggingErrors_CanceledTaskWithLogger_LogsError(bool generic)
        {
            _taskStub.SetCanceled();
            var loggerWrapper = new AwaitableLoggerWrapper(_logger);

            var wrappedTask = generic
                ? _taskStub.AsGenericTask.WithLoggingErrors<object>(loggerWrapper)
                : _taskStub.AsVoidTask.WithLoggingErrors(loggerWrapper);

            await Assert.ThrowsAsync<TaskCanceledException>(() => wrappedTask);

            await loggerWrapper.AwaitCalls;

            _logger.Received(1).Error(Arg.Any<TaskCanceledException>());
        }

        [Theory(Timeout = 10000)]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithLoggingErrors_FailedTaskWithLogger_LogsTwoErrors(bool generic)
        {
            _taskStub.SetException(new CommonTestException());
            var loggerWrapper = new AwaitableLoggerWrapper(_logger);

            var wrappedTask = generic
                ? _taskStub.AsGenericTask.WithLoggingErrors<object>(loggerWrapper)
                : _taskStub.AsVoidTask.WithLoggingErrors(loggerWrapper);

            await Assert.ThrowsAsync<CommonTestException>(() => wrappedTask);

            await loggerWrapper.AwaitCalls;

            _logger.Received(1).Error(Arg.Any<AggregateException>());
            _logger.Received(1).Error(Arg.Any<CommonTestException>());
        }

        [Theory(Timeout = 10000)]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithLoggingErrors_FailedTaskWithLoggerAndTasksComposition_LogsTwoErrors(bool generic)
        {
            _taskStub.SetException(new CommonTestException());
            var loggerWrapper = new AwaitableLoggerWrapper(_logger);

            var timeoutTask = generic
                ? _taskStub.AsGenericTask.WithLoggingErrors<object>(loggerWrapper).WithTimeoutAsync<object>(ShortTimeout)
                : _taskStub.AsVoidTask.WithLoggingErrors(loggerWrapper).WithTimeoutAsync(ShortTimeout);

            await Assert.ThrowsAsync<CommonTestException>(() => timeoutTask);

            await loggerWrapper.AwaitCalls;

            _logger.Received(1).Error(Arg.Any<AggregateException>());
            _logger.Received(1).Error(Arg.Any<CommonTestException>());
        }
    }
}
