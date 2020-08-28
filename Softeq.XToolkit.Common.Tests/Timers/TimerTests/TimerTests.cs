// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Timers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Timers.TimerTests
{
    public class TimerTests
    {
        private const int Interval = 100;
        private const int Delay = 200;
        private readonly Func<Task> _function = Substitute.For<Func<Task>>();

        [Fact]
        public void Constructor_WithDefaultParams_IsNotActive()
        {
            var timer = CreateTimerWithNullTask();

            Assert.NotNull(timer);
            Assert.False(timer.IsActive);
        }

        [Fact]
        public void Constructor_WithParams_IsNotActive()
        {
            var timer = CreateTimer();

            Assert.NotNull(timer);
            Assert.False(timer.IsActive);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Constructor_WithInvalidInterval_ThrowArgumentException(int interval)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var timer = new Timer(null, interval);
            });
        }

        [Fact]
        public void Start_WithNullTask_IsActive()
        {
            var timer = CreateTimerWithNullTask();

            timer.Start();

            Assert.True(timer.IsActive);
            timer.Stop();
        }

        [Fact]
        public async Task Start_WithTask_CallsFunctionAndIsActive()
        {
            var timer = CreateTimer();

            timer.Start();
            await Task.Delay(Delay);

            Assert.True(timer.IsActive);
            _function.ReceivedWithAnyArgs();
            timer.Stop();
        }

        [Fact]
        public async Task Start_AfterStart_CallsFunctionAndIsActive()
        {
            var timer = CreateTimer();

            timer.Start();
            timer.Start();
            await Task.Delay(Delay);

            Assert.True(timer.IsActive);
            _function.ReceivedWithAnyArgs();
            timer.Stop();
        }

        [Fact]
        public async Task Start_AfterStop_CallsFunctionAndIsActive()
        {
            var timer = CreateTimer();

            timer.Start();
            timer.Stop();
            _function.ClearReceivedCalls();
            timer.Start();
            await Task.Delay(Delay);

            Assert.True(timer.IsActive);
            _function.ReceivedWithAnyArgs();
            timer.Stop();
        }

        [Fact]
        public async Task Start_AfterDispose_DoesNotCallFunction()
        {
            var timer = CreateTimer();

            timer.Dispose();
            timer.Start();
            await Task.Delay(Delay);

            Assert.True(timer.IsActive);
            _function.DidNotReceiveWithAnyArgs();
            timer.Stop();
        }

        [Fact]
        public void Stop_WithoutStart_IsNotActive()
        {
            var timer = CreateTimer();

            timer.Stop();

            Assert.False(timer.IsActive);
        }

        [Fact]
        public async Task Stop_AfterStart_IsNotActive()
        {
            var timer = CreateTimer();

            timer.Start();
            timer.Stop();
            await Task.Delay(Delay);

            Assert.False(timer.IsActive);
            _function.DidNotReceiveWithAnyArgs();
        }

        [Fact]
        public void Dispose_WithoutStart_IsNotActive()
        {
            var timer = CreateTimer();

            timer.Dispose();

            Assert.False(timer.IsActive);
        }

        [Fact]
        public void Dispose_AfterStart_IsNotActive()
        {
            var timer = CreateTimer();

            timer.Start();
            timer.Dispose();

            Assert.False(timer.IsActive);
        }

        [Fact]
        public void Dispose_AfterStop_IsNotActive()
        {
            var timer = CreateTimer();

            timer.Start();
            timer.Stop();
            timer.Dispose();

            Assert.False(timer.IsActive);
        }

        private Timer CreateTimerWithNullTask()
        {
            var timer = new Timer(null, Interval);

            return timer;
        }

        private Timer CreateTimer()
        {
            var timer = new Timer(_function, Interval);

            return timer;
        }
    }
}
