// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    internal class DialogResult : IDialogResult
    {
        private readonly TaskCompletionSource<bool> _dismissCompletionSource;

        public DialogResult(TaskCompletionSource<bool> dismissCompletionSource)
        {
            _dismissCompletionSource = dismissCompletionSource;
        }

        public Task WaitDismissAsync => _dismissCompletionSource.Task;
    }

    internal sealed class DialogResult<T> : DialogResult, IDialogResult<T>
    {
        public DialogResult(T value, TaskCompletionSource<bool> dismissCompletionSource)
            : base(dismissCompletionSource)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
