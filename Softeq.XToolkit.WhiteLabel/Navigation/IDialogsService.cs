// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogsService
    {
        Task ShowDialogAsync(AlertDialogConfig config);

        Task<bool> ShowDialogAsync(ConfirmDialogConfig config);

        Task<string> ShowDialogAsync(ActionSheetDialogConfig config);

        Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel;

        Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel;
    }
}
