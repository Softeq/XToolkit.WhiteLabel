// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     Model representing the result of the dialog.
    /// </summary>
    public class DialogResult : IDialogResult
    {
        private readonly Task<bool> _dialogDismissTask;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogResult"/> class.
        /// </summary>
        /// <param name="dialogDismissTask">
        ///     The task that will be executed when the dialog is about to be dismissed.
        /// </param>
        public DialogResult(Task<bool> dialogDismissTask)
        {
            _dialogDismissTask = dialogDismissTask;
        }

        /// <summary>
        ///     Gets a task that will be executed when the dialog is about to be dismissed.
        ///     Await this task to identify when the dialog is dismissed.
        /// </summary>
        public Task WaitDismissAsync => _dialogDismissTask;
    }

    /// <summary>
    ///     Model representing the result of the dialog.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the result.
    /// </typeparam>
    public sealed class DialogResult<T> : DialogResult, IDialogResult<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogResult{T}"/> class.
        /// </summary>
        /// <param name="dialogDismissTask">
        ///     The task that will be executed when the dialog is about to be dismissed.
        /// </param>
        /// <param name="value">
        ///     Value representing the dialog result.
        /// </param>
        public DialogResult(T value, Task<bool> dialogDismissTask)
            : base(dialogDismissTask)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets a value representing the dialog result.
        /// </summary>
        public T Value { get; }
    }
}
