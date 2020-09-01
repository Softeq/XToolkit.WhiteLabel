// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Common.Timers
{
    /// <summary>
    ///     Runs and wait task after a set interval.
    /// </summary>
    public class Timer : ITimer, IDisposable
    {
        private readonly ILogger? _logger;
        private readonly int _interval;
        private Func<Task>? _taskFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Timer"/> class with specified interval.
        /// </summary>
        /// <param name="taskFactory">Returns Task to be executed at specified interval.</param>
        /// <param name="interval">Timer interval (ms).</param>
        /// <param name="logger">Optional Logger implementation.</param>
        public Timer(Func<Task> taskFactory, int interval, ILogger? logger = null)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("Interval should be a positive number");
            }

            _taskFactory = taskFactory;
            _interval = interval;
            _logger = logger;
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:Softeq.XToolkit.Common.Timers.Timer" /> should run task.
        /// </summary>
        /// <value><c>true</c> if Task should run; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; }

        /// <summary>
        ///     Releases all resource used by the <see cref="T:Softeq.XToolkit.Common.Timers.Timer" /> object.
        /// </summary>
        /// <remarks>
        ///     Call <see cref="Dispose" /> when you are finished using the <see cref="T:Softeq.XToolkit.Common.Timers.Timer" />. The
        ///     <see cref="Dispose" /> method leaves the <see cref="T:Softeq.XToolkit.Common.Timers.Timer" /> in an unusable state.
        ///     After calling <see cref="Dispose" />, you must release all references to the
        ///     <see cref="T:Softeq.XToolkit.Common.Timers.Timer" /> so the garbage collector can reclaim the memory that the
        ///     <see cref="T:Softeq.XToolkit.Common.Timers.Timer" /> was occupying.
        /// </remarks>
        public void Dispose()
        {
            Stop();
            _taskFactory = null;
        }

        /// <inheritdoc />
        public void Start()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            DoWork().FireAndForget(_logger);
        }

        /// <inheritdoc />
        public void Stop()
        {
            IsActive = false;
        }

        private async Task DoWork()
        {
            do
            {
                await Task.Delay(_interval);
                if (IsActive && _taskFactory != null)
                {
                    await _taskFactory().ConfigureAwait(false);
                }
            }
            while (IsActive);
        }
    }
}
