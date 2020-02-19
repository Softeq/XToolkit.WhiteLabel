// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public interface IAlertBuilder
    {
        Task<bool> ShowAlertAsync(string title, string message, string okButtonText, string? cancelButtonText = null);
    }
}