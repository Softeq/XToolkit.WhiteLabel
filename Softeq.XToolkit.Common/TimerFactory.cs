// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common
{
	public class TimerFactory : ITimerFactory
	{
        /// <summary>
        /// Create timer instance with the specified taskReference and interval.
        /// </summary>
        /// <returns>Timer instance.</returns>
        /// <param name="taskReference">Task to be executed at specified interval.</param>
        /// <param name="interval">Timer interval (ms).</param>
		public ITimer Create(TaskReference taskReference, int interval)
		{
			return new Timer(taskReference, interval);
		}
	}
}