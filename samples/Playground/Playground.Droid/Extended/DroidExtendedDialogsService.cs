// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Extended;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Droid.Extended
{
    public class DroidExtendedDialogsService : IExtendedDialogsService
    {
        private readonly IDialogsService _dialogsService;

        public DroidExtendedDialogsService(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Task<DateTime> ShowDialogAsync(ChooseBetterDateDialogConfig config)
        {
            return new DroidChooseBetterDateDialog(config).ShowAsync();
        }

        public Task ShowDialogAsync(AlertDialogConfig config)
        {
            return _dialogsService.ShowDialogAsync(config);
        }

        public Task<bool> ShowDialogAsync(ConfirmDialogConfig config)
        {
            return _dialogsService.ShowDialogAsync(config);
        }

        public Task<string> ShowDialogAsync(ActionSheetDialogConfig config)
        {
            return _dialogsService.ShowDialogAsync(config);
        }

        public Task<IDialogResult> ShowForViewModelAsync<TViewModel>(IEnumerable<NavigationParameterModel>? parameters = null) where TViewModel : IDialogViewModel
        {
            return _dialogsService.ShowForViewModelAsync<TViewModel>(parameters);
        }

        public Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(IEnumerable<NavigationParameterModel>? parameters = null) where TViewModel : IDialogViewModel
        {
            return _dialogsService.ShowForViewModelAsync<TViewModel, TResult>(parameters);
        }
    }
}
