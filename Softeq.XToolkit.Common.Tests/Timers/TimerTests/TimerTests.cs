// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Tasks;
using Softeq.XToolkit.Common.Timers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Timers.TimerTests
{
    public class TimerTests
    {
        [Fact]
        public void Constructor_WithDefaultParams_SetsParamsAndIsNotActive()
        {
            var interval = 0;

            var timer = new Timer(null, interval);

            Assert.NotNull(timer);
            Assert.Equal(interval, timer.Interval);
            Assert.Null(timer.TaskReference);
            Assert.False(timer.IsActive);
        }

        [Fact]
        public void Constructor_WithParams_SetsParamsAndIsNotActive()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 100;

            var timer = new Timer(taskRef, interval);

            Assert.NotNull(timer);
            Assert.Equal(interval, timer.Interval);
            Assert.Equal(taskRef, timer.TaskReference);
            Assert.False(timer.IsActive);
        }

        [Fact]
        public void Start_WithNullTask_ChangesIsActive()
        {
            var interval = 100;

            var timer = new Timer(null, interval);
            timer.Start();

            Assert.True(timer.IsActive);

            timer.Stop();
        }

        [Fact]
        public async Task Start_WithTask_CallsFunctionAndIsActive()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 10;
            var delay = 100;

            var timer = new Timer(taskRef, interval);
            timer.Start();
            await Task.Delay(delay);

            Assert.True(timer.IsActive);
            Assert.True(functionClass.ActionCount > 0, "Expected function to be called at least once");

            timer.Stop();
        }

        [Fact]
        public async Task Start_AfterStop_CallsFunctionAndIsActive()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 10;
            var delay = 100;

            var timer = new Timer(taskRef, interval);
            timer.Start();
            timer.Stop();
            functionClass.ActionCount = 0;
            timer.Start();
            await Task.Delay(delay);

            Assert.True(timer.IsActive);
            Assert.True(functionClass.ActionCount > 0, "Expected function to be called at least once");

            timer.Stop();
        }

        [Fact]
        public void Stop_WithoutStart_IsNotActive()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 100;

            var timer = new Timer(taskRef, interval);
            timer.Stop();

            Assert.False(timer.IsActive);
            Assert.NotNull(timer.TaskReference);
        }

        [Fact]
        public async Task Stop_AfterStart_IsNotActive()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 10;
            var delay = 100;
            var expectedCount = 0;

            var timer = new Timer(taskRef, interval);
            timer.Start();
            timer.Stop();
            await Task.Delay(delay);

            Assert.False(timer.IsActive);
            Assert.NotNull(timer.TaskReference);
            Assert.Equal(expectedCount, functionClass.ActionCount);
        }

        [Fact]
        public void Dispose_WithoutStart_IsNotActiveAndCleared()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 100;

            var timer = new Timer(taskRef, interval);
            timer.Dispose();

            Assert.False(timer.IsActive);
            Assert.Null(timer.TaskReference);
        }

        [Fact]
        public void Dispose_AfterStart_IsNotActiveAndCleared()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 100;

            var timer = new Timer(taskRef, interval);
            timer.Start();
            timer.Dispose();

            Assert.False(timer.IsActive);
            Assert.Null(timer.TaskReference);
        }

        [Fact]
        public void Dispose_AfterStop_IsNotActiveAndCleared()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 100;

            var timer = new Timer(taskRef, interval);
            timer.Start();
            timer.Stop();
            timer.Dispose();

            Assert.False(timer.IsActive);
            Assert.Null(timer.TaskReference);
        }
    }
}
