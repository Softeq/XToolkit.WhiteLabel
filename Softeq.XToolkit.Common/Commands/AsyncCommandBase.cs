// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     A command whose sole purpose is to relay its functionality to other
    ///     objects by invoking async delegates. The default return value for the CanExecute
    ///     method is 'true'. This class does not allow you to accept command parameters in the
    ///     Execute and CanExecute callback methods.
    /// </summary>
    public abstract class AsyncCommandBase
    {
        private readonly WeakAction<Exception>? _onException;

        protected AsyncCommandBase(Action<Exception>? onException)
        {
            if (onException != null)
            {
                _onException = new WeakAction<Exception>(onException);
            }
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        protected bool IsRunning { get; private set; }

        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected async Task DoExecuteAsync(Func<Task> executionProvider)
        {
            IsRunning = true;
            RaiseCanExecuteChanged();

            try
            {
                await executionProvider
                    .Invoke()
                    .ConfigureAwait(false);
            }
            finally
            {
                IsRunning = false;
                RaiseCanExecuteChanged();
            }
        }

        protected void TryHandleException(Exception e)
        {
            _onException?.Execute(e);
        }
    }
}
