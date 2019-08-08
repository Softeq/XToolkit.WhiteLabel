// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogResult
    {
        Task WaitDismissAsync { get; }
    }

    public interface IDialogResult<T> : IDialogResult
    {
        T Value { get; }
    }
}
