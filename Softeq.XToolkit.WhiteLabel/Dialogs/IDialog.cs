// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    public interface IDialog<T>
    {
        Task<T> ShowAsync();
    }
}
