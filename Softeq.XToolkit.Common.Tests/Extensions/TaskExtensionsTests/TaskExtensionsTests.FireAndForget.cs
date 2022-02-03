// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    public partial class TaskExtensionsTests
    {
        [Fact]
        public void FireAndForget_FailedTaskAndNoLogger_CallsWithoutException()
        {
            _taskStub.SetException(new CommonTestException());

            _taskStub.AsGenericTask.FireAndForget();
        }

        [Fact]
        public void FireAndForget_CompletedTaskAndNoLogger_CallsWithoutException()
        {
            _taskStub.SetResult(null);

            _taskStub.AsGenericTask.FireAndForget();
        }

        [Fact]
        public void FireAndForget_FailedTaskAndLoggerIsNull_CallsWithoutException()
        {
            _taskStub.SetException(new CommonTestException());

            _taskStub.AsGenericTask.FireAndForget(null as ILogger);
        }

        [Fact]
        public void FireAndForget_CompletedTaskAndLoggerIsNull_CallsWithoutException()
        {
            _taskStub.SetResult(null);

            _taskStub.AsGenericTask.FireAndForget(null as ILogger);
        }

        [Fact]
        public async Task FireAndForget_CompletedTaskWithLogger_ExecutesWithoutLogError()
        {
            _taskStub.SetResult(null);

            _taskStub.AsGenericTask.FireAndForget(_logger);

            await _taskStub.AwaitCompletionAsync();
            _logger.DidNotReceiveWithAnyArgs().Error(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_FailedTaskWithLogger_ExecutesWithLogError()
        {
            _taskStub.SetException(new CommonTestException());

            _taskStub.AsGenericTask.FireAndForget(_logger);

            await _taskStub.AwaitCompletionWithIgnoreExceptionAsync<CommonTestException>();
            _logger.Received(1).Error(Arg.Any<CommonTestException>());
        }

        [Fact]
        public async Task FireAndForget_CancelledTaskWithLogger_ExecutesWithLogError()
        {
            _taskStub.SetCanceled();

            _taskStub.AsGenericTask.FireAndForget(_logger);

            await _taskStub.AwaitCompletionWithIgnoreExceptionAsync<TaskCanceledException>();
            _logger.Received(1).Error(Arg.Any<TaskCanceledException>());
        }

        [Fact]
        public void FireAndForget_FailedTaskAndLoggerDelegateIsNull_CallsWithoutException()
        {
            _taskStub.SetException(new CommonTestException());

            _taskStub.AsGenericTask.FireAndForget(null as Action<Exception>);
        }

        [Fact]
        public void FireAndForget_CompletedTaskAndLoggerDelegateIsNull_CallsWithoutException()
        {
            _taskStub.SetResult(null);

            _taskStub.AsGenericTask.FireAndForget(null as Action<Exception>);
        }

        [Fact]
        public async Task FireAndForget_CompletedTaskAndLoggerDelegate_ExecutesWithoutLogError()
        {
            _taskStub.SetResult(null);
            var onException = Substitute.For<Action<Exception>>();

            _taskStub.AsGenericTask.FireAndForget(onException);

            await _taskStub.AwaitCompletionAsync();
            onException.DidNotReceive().Invoke(Arg.Any<Exception>());
        }

        [Fact]
        public async Task FireAndForget_CancelledTaskAndLoggerDelegate_ExecutesWithLogError()
        {
            _taskStub.SetCanceled();
            var onException = Substitute.For<Action<Exception>>();

            _taskStub.AsGenericTask.FireAndForget(onException);

            await _taskStub.AwaitCompletionWithIgnoreExceptionAsync<TaskCanceledException>();
            onException.Received(1).Invoke(Arg.Any<TaskCanceledException>());
        }
    }
}
