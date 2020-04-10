// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     An implementation of <see cref="IAsyncCommand"/>.
    ///     Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public class AsyncCommand : AsyncCommandBase, IAsyncCommand
    {
        private readonly Func<object, Task> _execute;

        public AsyncCommand(Func<Task> execute)
            : this(_ => execute(), _ => true)
        {
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute?.Invoke() ?? true)
        {
        }

        public AsyncCommand(Func<object, Task> execute, Func<object?, bool>? canExecute = null)
            : base(canExecute)
        {
            _execute = execute;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference.
        /// </param>
        public void Execute(object? parameter)
        {
            ExecuteAsync(parameter).FireAndForget(); // TODO YP: add onException
        }

        /// <inheritdoc cref="IAsyncCommand.ExecuteAsync"/>
        public async Task ExecuteAsync(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            IsRunning = true;

            try
            {
                await _execute(parameter);
            }
            finally
            {
                IsRunning = false;
                RaiseCanExecuteChanged();
            }
        }
    }
}
