// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Timers
{
    public class TimerFactory : ITimerFactory
    {
        /// <inheritdoc />
        public ITimer Create(Func<Task> taskFactory, int interval)
        {
            return new Timer(taskFactory, interval);
        }
    }
}
