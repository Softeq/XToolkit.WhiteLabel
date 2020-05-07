// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     An implementation of <see cref="IAsyncCommand"/>.
    ///     Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public class AsyncCommand : AsyncCommandBase, IAsyncCommand, IRaisableCanExecute
    {
        private readonly WeakFunc<Task> _execute;
        private readonly WeakFunc<bool>? _canExecute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called. IMPORTANT: Note that closures are not supported
        ///     at the moment due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will be executed.</param>
        public AsyncCommand(Func<Task> execute, Action<Exception> onException)
            : this(execute, null, onException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called. IMPORTANT: Note that closures are not supported
        ///     at the moment due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will be executed.</param>
        public AsyncCommand(
            Func<Task> execute,
            Func<bool>? canExecute = null,
            Action<Exception>? onException = null)
            : base(onException)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakFunc<Task>(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }

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
            AssertParameterNotUsed(parameter);

            if (!_execute.IsAlive)
            {
                return false;
            }

            if (IsRunning)
            {
                return false;
            }

            if (_canExecute == null)
            {
                return true;
            }

            if (!_canExecute.IsAlive)
            {
                return false;
            }

            return _canExecute.Execute();
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
            ExecuteAsync(parameter).FireAndForget(TryHandleException);
        }

        /// <inheritdoc cref="IAsyncCommand.ExecuteAsync"/>
        public Task ExecuteAsync(object? parameter)
        {
            return CanExecute(parameter)
                ? DoExecuteAsync(_execute.Execute)
                : Task.CompletedTask;
        }

        [Conditional("DEBUG")]
        private static void AssertParameterNotUsed(object? parameter)
        {
            if (parameter != null)
            {
                Debug.WriteLine($"WARNING: Command do not use parameter, but was provided with not-null value of type {parameter.GetType()}");
            }
        }
    }
}
