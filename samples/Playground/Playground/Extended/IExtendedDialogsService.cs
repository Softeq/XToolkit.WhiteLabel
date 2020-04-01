// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Extended
{
    public interface IExtendedDialogsService : IDialogsService
    {
        Task<DateTime> ShowDialogAsync(ChooseBetterDateDialogConfig config);
    }
}
