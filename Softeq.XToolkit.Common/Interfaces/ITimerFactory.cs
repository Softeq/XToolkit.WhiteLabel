// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
	public interface ITimerFactory
	{
		ITimer Create(TaskReference taskReference, int interval);
	}
}
