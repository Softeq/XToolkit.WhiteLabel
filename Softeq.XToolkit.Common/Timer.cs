// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Common
{
	public class Timer : ITimer, IDisposable
    {
        private readonly int _interval;
        private TaskReference _taskReference;

        public Timer(TaskReference taskReference, int interval)
        {
            _taskReference = taskReference;
            _interval = interval;
        }

        public bool IsActive { get; private set; }

        public void Dispose()
        {
            Stop();
            _taskReference = null;
        }

        public void Start()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            DoWork().SafeTaskWrapper();
        }

        public void Stop()
        {
            IsActive = false;
        }

        private async Task DoWork()
        {
            do
            {
                await Task.Delay(_interval);
                if (IsActive)
                {
                    await _taskReference.RunAsync().ConfigureAwait(false);
                }
            } while (IsActive);
        }
    }
}