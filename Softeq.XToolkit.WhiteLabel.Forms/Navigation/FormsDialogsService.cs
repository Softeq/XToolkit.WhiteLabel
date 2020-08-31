// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsDialogsService : IDialogsService
    {
        private readonly IContainer _container;
        private readonly IFormsViewLocator _viewLocator;

        public FormsDialogsService(
            IFormsViewLocator viewLocator,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _container = container;
        }

        public async Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string? cancelButtonText = null,
            OpenDialogOptions? options = null)
        {
            var page = GetLastPageInModalStack();
            if (string.IsNullOrEmpty(cancelButtonText))
            {
                await page.DisplayAlert(title, message, okButtonText).ConfigureAwait(false);
                return true;
            }

            var result = await page.DisplayAlert(title, message, okButtonText, cancelButtonText).ConfigureAwait(false);
            return result;
        }

        public Task ShowDialogAsync(AlertDialogConfig config)
        {
            var page = GetLastPageInModalStack();
            return page.DisplayAlert(config.Title, config.Message, config.CloseButtonText);
        }

        public Task<bool> ShowDialogAsync(ConfirmDialogConfig config)
        {
            var page = GetLastPageInModalStack();
            return page.DisplayAlert(
                config.Title,
                config.Message,
                config.AcceptButtonText,
                config.CancelButtonText);
        }

        public Task<string> ShowDialogAsync(ActionSheetDialogConfig config)
        {
            var page = GetLastPageInModalStack();
            return page.DisplayActionSheet(
                config.Title,
                config.CancelButtonText,
                config.DestructButtonText,
                config.OptionButtons);
        }

        public Task ShowForViewModel<TViewModel>(IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            throw new NotImplementedException();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            throw new NotImplementedException();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var result = await ShowForViewModelAsync<TViewModel, object>(parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var result = await ShowForViewModelImplAsync<TViewModel, TResult>(parameters).ConfigureAwait(false);
            return result;
        }

        private async Task<DialogResult<TResult>> ShowForViewModelImplAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _container.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);

            var view = await _viewLocator.GetPageAsync(viewModel);

            await GetModalNavigation()
                .PushModalAsync(view)
                .ConfigureAwait(false);

            var viewModelResult = await viewModel.DialogComponent.Task.ConfigureAwait(false);
            return new DialogResult<TResult>((TResult) viewModelResult, GetDismissTask(view));
        }

        private static Task<bool> GetDismissTask(Page page)
        {
            var lastPage = GetLastPageInModalStack();
            if (page == lastPage)
            {
                var tcs = new TaskCompletionSource<bool>();
                Execute.BeginOnUIThread(async () =>
                {
                    await lastPage.Navigation.PopModalAsync().ConfigureAwait(false);
                    tcs.TrySetResult(true);
                });
                return tcs.Task;
            }

            return Task.FromResult(true);
        }

        private static INavigation GetModalNavigation()
        {
            var page = GetLastPageInModalStack();
            return page.Navigation;
        }

        private static Page GetLastPageInModalStack()
        {
            // TODO VPY: check with multiple flows
            var navigation = Application.Current.MainPage.Navigation;
            return navigation.ModalStack.Count != 0
                ? navigation.ModalStack.Last()
                : navigation.NavigationStack.Last();
        }
    }
}
