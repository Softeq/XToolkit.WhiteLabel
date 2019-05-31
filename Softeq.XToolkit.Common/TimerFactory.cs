// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common
{
	public class TimerFactory : ITimerFactory
	{
		public ITimer Create(TaskReference taskReference, int interval)
		{
			return new Timer(taskReference, interval);
		}
	}
}