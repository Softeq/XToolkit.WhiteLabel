// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

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

        protected AsyncCommandBase(Func<object?, bool>? canExecute)
        {
            _canExecute = canExecute ?? (_ => true);
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        protected bool IsRunning { get; set; }

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
            return !IsRunning && _canExecute(parameter);
        }

        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
