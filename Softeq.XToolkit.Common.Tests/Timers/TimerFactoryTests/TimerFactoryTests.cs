// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
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
            var task = Substitute.For<Func<Task>>();
            var taskRef = new TaskReference(task);
            var interval = 1000;

            var result = _timerFactory.Create(taskRef, interval);

            Assert.NotNull(result);
            Assert.IsType<Timer>(result);
        }
    }
}
