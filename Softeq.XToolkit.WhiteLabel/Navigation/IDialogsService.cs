// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogsService
    {
        [Obsolete("Use ShowDialogAsync(new ConfirmDialogConfig()) instead!")]
        Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string? cancelButtonText = null,
            OpenDialogOptions? options = null);

        Task ShowDialogAsync(AlertDialogConfig config);

        Task<bool> ShowDialogAsync(ConfirmDialogConfig config);

        Task<string> ShowDialogAsync(ActionSheetDialogConfig config);

        [Obsolete("Please use ShowForViewModelAsync syntax instead")]
        Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel;

        [Obsolete("Please use ShowForViewModelAsync syntax instead")]
        Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel;

        Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel;

        Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel;
    }
}
