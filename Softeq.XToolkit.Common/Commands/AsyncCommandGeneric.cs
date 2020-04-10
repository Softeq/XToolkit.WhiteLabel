// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     An implementation of <see cref="IAsyncCommand"/>.
    ///     Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    /// <typeparam name="T">Type of parameter.</typeparam>
    [SuppressMessage("ReSharper", "SA1649", Justification = "File name is fine")]
    public class AsyncCommand<T> : AsyncCommandBase, IAsyncCommand<T>
    {
        private readonly Func<T, Task> _execute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The Function executed when Execute or ExecuteAsync is called.
        ///     This does not check canExecute before executing and will execute even if canExecute is false.
        /// </param>
        public AsyncCommand(Func<T, Task> execute)
            : this(execute, _ => true)
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
        public AsyncCommand(Func<T, Task> execute, Func<bool> canExecute)
            : this(execute, _ => canExecute?.Invoke() ?? true)
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
        public AsyncCommand(Func<T, Task> execute, Func<object?, bool>? canExecute = null)
            : base(canExecute)
        {
            _execute = execute;
        }

        /// <inheritdoc cref="AsyncCommandBase.CanExecute"/>
        public bool CanExecute(T parameter)
        {
            return CanExecute(parameter as object);
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        /// <typeparam name="T">Type of parameter.</typeparam>
        public void Execute(T parameter)
        {
            ExecuteInternal(parameter);
        }

        /// <inheritdoc cref="AsyncCommand.Execute"/>
        public void Execute(object? parameter)
        {
            ExecuteInternal(parameter);
        }

        public async Task ExecuteAsync(T parameter)
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

        protected virtual void ExecuteInternal<TParameter>(TParameter parameter)
        {
            switch (parameter)
            {
                case T validParameter:
                    ExecuteAsync(validParameter).FireAndForget(); // TODO YP: add onException
                    break;

                case null when !typeof(T).GetTypeInfo().IsValueType:
                    ExecuteAsync(default).FireAndForget(); // TODO YP: add onException
                    break;

                case null:
                    throw new InvalidCommandParameterException(typeof(T));

                default:
                    throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
            }
        }
    }
}
