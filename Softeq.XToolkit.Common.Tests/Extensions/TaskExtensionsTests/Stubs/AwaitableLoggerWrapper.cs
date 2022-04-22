// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests.Stubs
{
    internal class AwaitableLoggerWrapper : ILogger
    {
        private readonly ILogger _logger;
        private readonly TaskCompletionSource<bool> _tcs;

        public AwaitableLoggerWrapper(ILogger logger)
        {
            _logger = logger;
            _tcs = new TaskCompletionSource<bool>();
        }

        public Task AwaitCalls => _tcs.Task;

        public void Debug(string message)
        {
            _logger.Debug(message);
            FinishTracking();
        }

        public void Info(string message)
        {
            _logger.Info(message);
            FinishTracking();
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
            FinishTracking();
        }

        public void Warn(Exception exception)
        {
            _logger.Warn(exception);
            FinishTracking();
        }

        public void Error(string message)
        {
            _logger.Error(message);
            FinishTracking();
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
            FinishTracking();
        }

        /// <summary>
        ///     Finish task by first call.
        /// </summary>
        private void FinishTracking()
        {
            if (!_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(true);
            }
        }
    }
}
