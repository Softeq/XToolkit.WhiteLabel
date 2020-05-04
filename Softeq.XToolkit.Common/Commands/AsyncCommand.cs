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
    public class AsyncCommand : AsyncCommandBase, IAsyncCommand, IRaisableCanExecute
    {
        private readonly Func<object, Task> _execute;
        private readonly Action<Exception>? _onException;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        public AsyncCommand(Func<Task> execute)
            : this(_ => execute(), _ => true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public AsyncCommand(Func<Task> execute, Action<Exception> onException)
            : this(_ => execute(), _ => true, onException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute, Action<Exception>? onException = null)
            : this(_ => execute(), _ => canExecute?.Invoke() ?? true, onException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public AsyncCommand(
            Func<object, Task> execute,
            Func<object?, bool>? canExecute,
            Action<Exception>? onException = null)
            : base(canExecute)
        {
            _execute = execute;
            _onException = onException;
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
            ExecuteAsync(parameter).FireAndForget(_onException!);
        }

        /// <inheritdoc cref="IAsyncCommand.ExecuteAsync"/>
        public Task ExecuteAsync(object? parameter)
        {
            return ExecuteAsync(_execute, parameter!);
        }
    }
}
