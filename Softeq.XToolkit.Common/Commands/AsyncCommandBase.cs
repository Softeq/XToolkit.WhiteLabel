// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     A command whose sole purpose is to relay its functionality to other
    ///     objects by invoking async delegates. The default return value for the CanExecute
    ///     method is 'true'.  This class does not allow you to accept command parameters in the
    ///     Execute and CanExecute callback methods.
    /// </summary>
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly WeakFunc<bool>? _canExecute;
        private bool _isRunning;

        protected AsyncCommandBase(Func<bool>? canExecute)
        {
            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }

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
            var canExecute = _canExecute == null
                             || ((_canExecute.IsStatic || _canExecute.IsAlive)
                             && _canExecute.Execute());
            return !_isRunning && canExecute;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference.
        /// </param>
        public virtual void Execute(object? parameter)
        {
            ExecuteAsync(parameter).FireAndForget();
        }

        public async Task ExecuteAsync(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            _isRunning = true;

            try
            {
                await ExecuteAsyncImpl(parameter).Invoke();
            }
            finally
            {
                _isRunning = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected abstract Func<Task> ExecuteAsyncImpl(object? parameter);
    }
}
