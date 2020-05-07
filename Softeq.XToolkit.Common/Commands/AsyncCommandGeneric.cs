// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called. IMPORTANT: Note that closures are not supported
        ///     at the moment due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will be executed.</param>
        public AsyncCommand(Func<T, Task> execute, Action<Exception> onException)
            : this(execute, null, onException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called. IMPORTANT: Note that closures are not supported
        ///     at the moment due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will be executed.</param>
        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool>? canExecute = null,
            Action<Exception>? onException = null)
            : base(onException)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakFunc<T, Task>(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<T, bool>(canExecute);
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

            return _canExecute.Execute(parameter);
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
            return TryParseParameter(parameter, out T parsed) && CanExecute(parsed);
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        public void Execute(object? parameter)
        {
            if (TryParseParameter(parameter, out T parsed))
            {
                Execute(parsed);
            }
            else
            {
                AssertParameterNotSupported(parameter);
            }
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        /// <typeparam name="T">Type of parameter.</typeparam>
        public void Execute(T parameter)
        {
            ExecuteAsync(parameter).FireAndForget(TryHandleException);
        }

        /// <inheritdoc cref="IAsyncCommand.ExecuteAsync"/>
        /// <typeparam name="T">Type of parameter.</typeparam>
        public Task ExecuteAsync(T parameter)
        {
            return CanExecute(parameter)
                ? DoExecuteAsync(() => _execute.Execute(parameter))
                : Task.CompletedTask;
        }

        private static bool TryParseParameter(object? parameter, out T parsed)
        {
            switch (parameter)
            {
                case T p:
                    parsed = p;
                    return true;
                case null when !typeof(T).GetTypeInfo().IsValueType:
                    parsed = default!;
                    return true;
                default:
                    parsed = default!;
                    return false;
            }
        }

        [Conditional("DEBUG")]
        private static void AssertParameterNotSupported(object? parameter)
        {
            var parameterFormatted = parameter != null
                ? $"of type {parameter.GetType()}"
                : $"\"null\"";

            Debug.WriteLine($"WARNING: Command cannot be executed with parameter {parameterFormatted}; type {typeof(T)} is expected");
        }
    }
}
