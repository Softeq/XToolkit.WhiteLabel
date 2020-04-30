// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     An implementation of <see cref="IAsyncCommand"/>.
    ///     Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    /// <typeparam name="T">Type of parameter.</typeparam>
    [SuppressMessage("ReSharper", "SA1649", Justification = "File name is fine")]
    public class AsyncCommand<T> : AsyncCommandBase, IAsyncCommand<T>, IRaisableCanExecute
    {
        private readonly WeakFunc<T, Task> _execute;
        private readonly WeakFunc<T, bool>? _canExecute;
        private readonly WeakAction<Exception>? _onException;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public AsyncCommand(Func<T, Task> execute, Action<Exception> onException)
            : this(execute, null, onException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool>? canExecute = null,
            Action<Exception>? onException = null)
        {
            _execute = new WeakFunc<T, Task>(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<T, bool>(canExecute);
            }

            if (onException != null)
            {
                _onException = new WeakAction<Exception>(onException);
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
        public bool CanExecute(T parameter)
        {
            return !IsRunning && (_canExecute?.Execute(parameter) ?? true);
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
            switch (parameter)
            {
                case T validParameter:
                    return CanExecute(validParameter);

                case null when !typeof(T).GetTypeInfo().IsValueType:
                    return CanExecute(default!);

                case null:
                    throw new InvalidCommandParameterException(typeof(T));

                default:
                    throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
            }
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        public void Execute(object? parameter)
        {
            switch (parameter)
            {
                case T validParameter:
                    Execute(validParameter);
                    break;

                case null when !typeof(T).GetTypeInfo().IsValueType:
                    Execute(default!);
                    break;

                case null:
                    throw new InvalidCommandParameterException(typeof(T));

                default:
                    throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
            }
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        /// <typeparam name="T">Type of parameter.</typeparam>
        public void Execute(T parameter)
        {
            ExecuteAsync(parameter).FireAndForget(TryHandleException);
        }

        public Task ExecuteAsync(T parameter)
        {
            return CanExecute(parameter)
                ? DoExecuteAsync(() => _execute.Execute(parameter))
                : Task.CompletedTask;
        }

        private void TryHandleException(Exception e)
        {
            _onException?.Execute(e);
        }
    }
}
