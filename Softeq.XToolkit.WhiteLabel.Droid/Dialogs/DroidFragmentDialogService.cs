// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        public DroidFragmentDialogService(
            IViewLocator viewLocator,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
        }

        public Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string cancelButtonText = null,
            OpenDialogOptions openDialogOptions = null)
        {
            var actions = new Dictionary<bool, DialogOption>
            {
                { true, new DialogOption { Title = okButtonText } }
            };
            if (cancelButtonText != null)
            {
                actions.Add(false, new DialogOption { Title = cancelButtonText, CommandActionStyle = CommandActionStyle.Cancel });
            }
            return ShowDialogAsync(title, message, actions);
        }

        public Task<TResult> ShowDialogAsync<TResult>(string title, string message, Dictionary<TResult, DialogOption> actions)
        {
            var tcs = new TaskCompletionSource<TResult>();

            Execute.BeginOnUIThread(() =>
            {
                var context = CrossCurrentActivity.Current.Activity;

                var builder = new AlertDialog.Builder(context)
                    .SetTitle(title)
                    .SetMessage(message)
                    .SetCancelable(typeof(TResult) == typeof(bool));

                foreach (var action in actions)
                {
                    EventHandler<DialogClickEventArgs> onOptionChosen = (o, e) =>
                    {
                        tcs.TrySetResult(action.Key);
                        var alertDialog = (AlertDialog) o;
                        alertDialog.Dismiss();
                    };
                    if (action.Value.CommandActionStyle == CommandActionStyle.Destructive)
                    {
                        builder.SetPositiveButton(action.Value.Title, onOptionChosen);
                    }
                    else if (action.Value.CommandActionStyle == CommandActionStyle.Default)
                    {
                        builder.SetNeutralButton(action.Value.Title, onOptionChosen);
                    }
                    else
                    {
                        builder.SetNegativeButton(action.Value.Title, onOptionChosen);
                    }
                }

                var dialog = builder.Create();
                var dismissCommand = new RelayCommand<TaskCompletionSource<TResult>>(x => { x.TrySetResult(default(TResult)); });
                dialog.SetCommand(nameof(dialog.DismissEvent), dismissCommand, tcs);
                dialog.Show();
            });

            return tcs.Task;
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = CreateViewModel<TViewModel>(parameters);

            await ShowDialogAsync(viewModel);

            return new DialogResult(Task.FromResult(true));
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
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
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
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