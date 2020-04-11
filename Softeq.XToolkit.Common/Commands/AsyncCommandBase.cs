// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

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
        private readonly Func<object?, bool> _canExecute;

        private bool _isRunning;

        protected AsyncCommandBase(Func<object?, bool>? canExecute)
        {
            _canExecute = canExecute ?? (_ => true);
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return !_isRunning && _canExecute(parameter);
        }

        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected async Task ExecuteAsync<T>(Func<T, Task> execute, T parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            _isRunning = true;

            try
            {
                await execute(parameter).ConfigureAwait(false);
            }
            finally
            {
                _isRunning = false;
                RaiseCanExecuteChanged();
            }
        }
    }
}
