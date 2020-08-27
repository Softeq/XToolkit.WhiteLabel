// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class DialogViewModelComponent
    {
        private readonly TaskCompletionSource<object> _withResultCompletionSource;

        public DialogViewModelComponent()
        {
            _withResultCompletionSource = new TaskCompletionSource<object>();

            CloseCommand = new RelayCommand<object>(DoClose);
        }

        public event EventHandler? Closed;

        public RelayCommand<object> CloseCommand { get; }

        public Task<object> Task => _withResultCompletionSource.Task;

        public void OnDismissed()
        {
            SetResult(null);
        }

        private void DoClose(object result)
        {
            SetResult(result);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void SetResult(object result)
        {
            _withResultCompletionSource.TrySetResult(result);
        }
    }
}
