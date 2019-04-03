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
        private readonly TaskCompletionSource<IDialogViewModel> _completionSource;
        private readonly TaskCompletionSource<object> _withResultCompletionSource;
        private readonly WeakReferenceEx<IDialogViewModel> _holderViewModelRef;

        public DialogViewModelComponent(IDialogViewModel holderViewModel)
        {
            _holderViewModelRef = WeakReferenceEx.Create(holderViewModel);
            _completionSource = new TaskCompletionSource<IDialogViewModel>();
            _withResultCompletionSource = new TaskCompletionSource<object>();
            CloseCommand = new RelayCommand<bool>(Close);
            CloseWithResultCommand = new RelayCommand<object>(Close);
        }

        public RelayCommand<bool> CloseCommand { get; }
        public RelayCommand<object> CloseWithResultCommand { get; }

        public Task<IDialogViewModel> Task => _completionSource.Task;
        public Task<object> TaskWithResult => _withResultCompletionSource.Task;

        public event EventHandler Closed;

        private void Close(object result)
        {
            _withResultCompletionSource.TrySetResult(result);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void Close(bool result)
        {
            _completionSource.TrySetResult(result ? _holderViewModelRef.Target : null);
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}