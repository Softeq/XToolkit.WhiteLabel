// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
	public interface ITimer
	{
		bool IsActive { get; }
		void Start();
		void Stop();
	}
}
