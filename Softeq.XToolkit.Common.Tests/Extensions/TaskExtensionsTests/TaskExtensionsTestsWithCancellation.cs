// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    public partial class TaskExtensionsTests
    {
        [Fact]
        public async Task WithCancellation_TaskWithCancelledToken_ThrowsTaskCanceledException()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var taskWithCancellation = _taskStub.AsGenericTask.WithCancellation(cancellationTokenSource.Token);

            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<TaskCanceledException>(() => taskWithCancellation);
        }

        [Fact]
        public async Task WithCancellation_CompletedTaskAndCancellationTokenIsNone_ReturnsResultWithoutCancellation()
        {
            _taskStub.SetResult(null);
            var task = _taskStub.AsGenericTask;

            var taskWithCancellation = task.WithCancellation(CancellationToken.None);

            await taskWithCancellation;
            Assert.Same(task.Result, taskWithCancellation.Result);
        }

        [Fact]
        public async Task WithCancellation_CompletedTaskAndCancellationToken_ReturnsResultWithoutCancellation()
        {
            var task = _taskStub.AsGenericTask;
            var cancellationTokenSource = new CancellationTokenSource();

            var taskWithCancellation = task.WithCancellation(cancellationTokenSource.Token);

            _taskStub.SetResult(null);

            await taskWithCancellation;
            Assert.Same(task.Result, taskWithCancellation.Result);
        }

        [Fact]
        public async Task WithCancellation_FailedTaskAndCancellationToken_ThrowsTaskException()
        {
            _taskStub.SetException(new CommonTestException());
            var task = _taskStub.AsGenericTask;
            var cancellationTokenSource = new CancellationTokenSource();

            var taskWithCancellation = task.WithCancellation(CancellationToken.None);

            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<CommonTestException>(() => taskWithCancellation);
        }
    }
}
