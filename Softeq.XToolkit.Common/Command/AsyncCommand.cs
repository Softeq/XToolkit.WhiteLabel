using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Softeq.XToolkit.Common.Command
{
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly WeakFunc<bool> _canExecute;
        private bool _isRunning;

        protected AsyncCommandBase(Func<bool> canExecute)
        {
            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }

        public bool CanExecute(object parameter)
        {
            var canExecute = _canExecute == null
                   || (_canExecute.IsStatic || _canExecute.IsAlive)
                   && _canExecute.Execute();
            return !_isRunning && canExecute;
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            _isRunning = true;

            try
            {
                await ExecuteAsync(parameter).Invoke();
            }
            finally
            {
                _isRunning = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected abstract Func<Task> ExecuteAsync(object parameter);

        public event EventHandler CanExecuteChanged;
    }

    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _action;

        public AsyncCommand(Func<Task> myAsyncFunction, Func<bool> canExecute = null) : base(canExecute)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsync(object parameter)
        {
            return _action;
        }
    }

    //TODO PL: add parameter to canExecute
    public class AsyncCommand<T> : AsyncCommandBase, ICommand<T>
    {
        private readonly Func<T, Task> _action;

        public AsyncCommand(Func<T, Task> myAsyncFunction, Func<bool> canExecute = null) : base(canExecute)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsync(object parameter)
        {
            return () => _action((T)parameter);
        }
    }
}
