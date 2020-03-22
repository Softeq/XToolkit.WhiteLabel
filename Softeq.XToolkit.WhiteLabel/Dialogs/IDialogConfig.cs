// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    // ReSharper disable once UnusedTypeParameter
    public interface IDialogConfig<T>
    {
    }

    public interface IDialog<T>
    {
        Task<T> ShowAsync();
    }
}
