// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
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
                ? TaskExt.WithTimeout<int>(null, TimeSpan.FromSeconds(1))
                : TaskExt.WithTimeout(null, TimeSpan.FromSeconds(1));

            Assert.Throws<ArgumentNullException>(() => timeoutTask.GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task WithTimeout_MinusOneMeansInfiniteTimeout(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeout<object>(TimeSpan.FromMilliseconds(-1))
                : ((Task) tcs.Task).WithTimeout(TimeSpan.FromMilliseconds(-1));

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
                ? tcs.Task.WithTimeout(TimeSpan.FromMilliseconds(1))
                : ((Task) tcs.Task).WithTimeout(TimeSpan.FromMilliseconds(1));

            Assert.Throws<TimeoutException>(() => timeoutTask.GetAwaiter().GetResult());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void WithTimeout_CompletesFirst(bool generic)
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = generic
                ? tcs.Task.WithTimeout(TimeSpan.FromDays(1))
                : ((Task) tcs.Task).WithTimeout(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetResult(null);

            timeoutTask.GetAwaiter().GetResult();
        }

        [Fact]
        public void WithTimeout_CompletesFirstWithResult()
        {
            var tcs = new TaskCompletionSource<object>();
            var timeoutTask = tcs.Task.WithTimeout(TimeSpan.FromDays(1));

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
                ? tcs.Task.WithTimeout(TimeSpan.FromDays(1))
                : ((Task) tcs.Task).WithTimeout(TimeSpan.FromDays(1));

            Assert.False(timeoutTask.IsCompleted);

            tcs.SetException(new ApplicationException());

            await Assert.ThrowsAsync<ApplicationException>(() => timeoutTask);
            Assert.Same(tcs.Task.Exception.InnerException, timeoutTask.Exception.InnerException);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SafeTaskWrapper_Execute(bool generic)
        {
            var logger = Substitute.For<ILogger>();
            var tcs = new TaskCompletionSource<object>();
            var task = generic
                ? tcs.Task.SafeTaskWrapper<object>(logger)
                : ((Task) tcs.Task).SafeTaskWrapper(logger);

            Assert.False(task.IsCompleted);

            tcs.SetResult(null);

            task.GetAwaiter().GetResult();

            logger.DidNotReceiveWithAnyArgs().Error(default(string));
        }
    }
}
