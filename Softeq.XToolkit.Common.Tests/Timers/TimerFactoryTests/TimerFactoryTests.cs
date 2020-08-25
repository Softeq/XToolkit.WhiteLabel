// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Tasks;
using Softeq.XToolkit.Common.Timers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Timers.TimerFactoryTests
{
    public class TimerFactoryTests
    {
        private readonly TimerFactory _timerFactory;

        public TimerFactoryTests()
        {
            _timerFactory = new TimerFactory();
        }

        [Fact]
        public void Create_WithParams_CreatedTimer()
        {
            var functionClass = new TestFunctionClass();
            var taskRef = new TaskReference(functionClass.Function);
            var interval = 1000;

            var result = _timerFactory.Create(taskRef, interval);

            Assert.NotNull(result);
            Assert.IsType<Timer>(result);
            Assert.Equal(interval, (result as Timer).Interval);
            Assert.Equal(taskRef, (result as Timer).TaskReference);
        }
    }
}
