// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
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
            var taskFactory = Substitute.For<Func<Task>>();
            var interval = 1000;

            var result = _timerFactory.Create(taskFactory, interval);

            Assert.NotNull(result);
            Assert.IsType<Timer>(result);
        }
    }
}
