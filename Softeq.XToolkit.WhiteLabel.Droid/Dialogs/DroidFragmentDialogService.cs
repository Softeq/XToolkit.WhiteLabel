// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IContainer _container;
        private readonly IViewLocator _viewLocator;

        public DroidFragmentDialogService(
            IViewLocator viewLocator,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _container = container;
        }

        [Obsolete("Use ShowDialogAsync(new ConfirmDialogConfig()) instead.")]
        public Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string? cancelButtonText = null,
            OpenDialogOptions? openDialogOptions = null)
        {
            return ShowDialogAsync(new ConfirmDialogConfig
            {
                Title = title,
                Message = message,
                AcceptButtonText = okButtonText,
                CancelButtonText = cancelButtonText
            });
        }

        public Task ShowDialogAsync(AlertDialogConfig config)
        {
            return new DroidAlertDialog(config).ShowAsync();
        }

        public Task<bool> ShowDialogAsync(ConfirmDialogConfig config)
        {
            return new DroidConfirmDialog(config).ShowAsync();
        }

        public Task<string> ShowDialogAsync(ActionSheetDialogConfig config)
        {
            return new DroidActionSheetDialog(config).ShowAsync();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = CreateViewModel<TViewModel>(parameters);

            await ShowDialogAsync(viewModel);

            return new DialogResult(Task.FromResult(true));
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = CreateViewModel<TViewModel>(parameters);

            var resultObject = await ShowDialogAsync(viewModel).ConfigureAwait(false);

            var result = resultObject is TResult convertedResult
                ? convertedResult
                : default;

            return new DialogResult<TResult>(result, Task.FromResult(true));
        }

        private TViewModel CreateViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _container.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            return viewModel;
        }

        private Task<object> ShowDialogAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : IDialogViewModel
        {
            var dialogFragment = (DialogFragmentBase<TViewModel>) _viewLocator.GetView(viewModel, ViewType.DialogFragment);
            dialogFragment.Show();
            return viewModel.DialogComponent.Task;
        }
    }
}
