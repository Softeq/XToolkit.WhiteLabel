using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Softeq.XToolkit.Common.Command
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isRunning;

        public virtual bool CanExecute(object parameter)
        {
            return !_isRunning;
        }

        public async void Execute(object parameter)
        {
            if (_isRunning)
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

        public AsyncCommand(Func<Task> myAsyncFunction)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsync(object parameter)
        {
            return _action;
        }
    }

    public class AsyncCommand<T> : AsyncCommandBase, ICommand<T>
    {
        private readonly Func<T, Task> _action;

        public AsyncCommand(Func<T, Task> myAsyncFunction)
        {
            _action = myAsyncFunction;
        }

        protected override Func<Task> ExecuteAsync(object parameter)
        {
            return () => _action((T)parameter);
        }
    }
}