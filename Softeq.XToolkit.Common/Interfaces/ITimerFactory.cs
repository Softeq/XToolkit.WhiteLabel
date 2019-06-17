// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
	public interface ITimerFactory
    {
        /// <summary>
        /// Initializes a new instance of the timer with specified interval.
        /// </summary>
        /// <param name="taskReference">Task to be executed at specified interval.</param>
        /// <param name="interval">Timer interval (ms).</param>
        /// <returns>Returns timer object.</returns>
		ITimer Create(TaskReference taskReference, int interval);
	}
}
