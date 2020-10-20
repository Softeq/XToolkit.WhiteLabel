// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     Dialog controller that contains all the required methods and properties
    ///     to close the dialog and track its state.
    /// </summary>
    public class DialogViewModelComponent
    {
        private readonly TaskCompletionSource<object> _withResultCompletionSource;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogViewModelComponent"/> class.
        /// </summary>
        public DialogViewModelComponent()
        {
            _withResultCompletionSource = new TaskCompletionSource<object>();

            CloseCommand = new RelayCommand<object>(DoClose);
        }

        /// <summary>
        ///     An event that will be raised once the dialog is closed.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        ///     Gets a command that should be executed to close a dialog.
        /// </summary>
        public RelayCommand<object> CloseCommand { get; }

        /// <summary>
        ///     Gets a task that represents a dialog state.
        ///     When the dialog is closed, the task will complete with the dialog result.
        /// </summary>
        public Task<object> Task => _withResultCompletionSource.Task;

        private void DoClose(object result)
        {
            _withResultCompletionSource.TrySetResult(result);
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
