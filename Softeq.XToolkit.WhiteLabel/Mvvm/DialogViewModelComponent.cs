// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class DialogViewModelComponent
    {
        private readonly TaskCompletionSource<object> _withResultCompletionSource;

        public DialogViewModelComponent()
        {
            _withResultCompletionSource = new TaskCompletionSource<object>();
            CloseCommand = new RelayCommand<object>(Close);
        }

        public RelayCommand<object> CloseCommand { get; }

        public Task<object> Task => _withResultCompletionSource.Task;

        public event EventHandler Closed;

        private void Close(object result)
        {
            _withResultCompletionSource.TrySetResult(result);
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}