// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    [Obsolete("Use DroidAlertDialog or DroidConfirmDialog instead.")]
    public interface IAlertBuilder
    {
        Task<bool> ShowAlertAsync(string title, string message, string okButtonText, string? cancelButtonText = null);
    }
}