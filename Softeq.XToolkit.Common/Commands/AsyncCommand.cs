// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;
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

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            var canExecute = _canExecute == null
                             || (_canExecute.IsStatic || _canExecute.IsAlive)
                             && _canExecute.Execute();
            return !_isRunning && canExecute;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference
        /// </param>
        public virtual void Execute(object? parameter)
        {
            ExecuteAsync(parameter).SafeTaskWrapper();
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

        public event EventHandler? CanExecuteChanged;

        protected abstract Func<Task> ExecuteAsyncImpl(object? parameter);
    }

    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Commands.AsyncCommand" /> class.
        /// </summary>
        /// <param name="myAsyncFunction">
        ///     The execution logic. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="ArgumentNullException">
        ///     If the execute argument is null. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </exception>
        public AsyncCommand(Func<Task> myAsyncFunction, Func<bool>? canExecute = null) : base(canExecute)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsyncImpl(object? parameter)
        {
            return _action;
        }
    }

    //TODO PL: add parameter to canExecute
    public class AsyncCommand<T> : AsyncCommandBase, ICommand<T>
    {
        private readonly Func<T, Task> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Softeq.XToolkit.Common.Commands.AsyncCommand" /> class.
        /// </summary>
        /// <param name="myAsyncFunction">
        ///     The execution logic. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="ArgumentNullException">
        ///     If the execute argument is null. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </exception>
        public AsyncCommand(Func<T, Task> myAsyncFunction, Func<bool>? canExecute = null) : base(canExecute)
        {
            _action = myAsyncFunction;
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (parameter == null && typeof(T).GetTypeInfo().IsValueType)
            {
                throw new ArgumentException($"Async command wait parameter with type: {typeof(T)}", nameof(parameter));
            }

            Execute(parameter == null ? default : (T) parameter);
        }

        protected override Func<Task> ExecuteAsyncImpl(object? parameter)
        {
            return () => _action(parameter == null ? default : (T) parameter);
        }
    }
}
