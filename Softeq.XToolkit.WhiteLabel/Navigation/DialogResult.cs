// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class DialogResult : IDialogResult
    {
        private readonly Task<bool> _dialogDismissTask;

        public DialogResult(Task<bool> dialogDismissTask)
        {
            _dialogDismissTask = dialogDismissTask;
        }

        public Task WaitDismissAsync => _dialogDismissTask;
    }

    public sealed class DialogResult<T> : DialogResult, IDialogResult<T>
    {
        public DialogResult(T value, Task<bool> dialogDismissTask) : base(dialogDismissTask)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
