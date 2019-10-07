// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Tasks;

namespace Softeq.XToolkit.Common.Timers
{
    public class TimerFactory : ITimerFactory
    {
        /// <inheritdoc />
        public ITimer Create(TaskReference taskReference, int interval)
        {
            return new Timer(taskReference, interval);
        }
    }
}
