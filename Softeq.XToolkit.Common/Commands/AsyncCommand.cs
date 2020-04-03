// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Commands
{
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
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
        public AsyncCommand(Func<Task> myAsyncFunction, Func<bool>? canExecute = null)
            : base(canExecute)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsyncImpl(object? parameter)
        {
            return _action;
        }
    }

    // TODO PL: add parameter to canExecute
    public class AsyncCommand<T> : AsyncCommandBase, ICommand<T>
    {
        private readonly Func<T, Task> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
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
        public AsyncCommand(Func<T, Task> myAsyncFunction, Func<bool>? canExecute = null)
            : base(canExecute)
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

            Execute(parameter == null ? default! : (T) parameter);
        }

        protected override Func<Task> ExecuteAsyncImpl(object? parameter)
        {
            return () => _action(parameter == null ? default! : (T) parameter);
        }
    }
}
