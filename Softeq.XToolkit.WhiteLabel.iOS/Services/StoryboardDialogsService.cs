// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardDialogsService : IDialogsService
    {
        private readonly ILogger _logger;
        private readonly IViewLocator _viewLocator;
        private readonly IContainer _container;

        public StoryboardDialogsService(
            IViewLocator viewLocator,
            ILogManager logManager,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _container = container;
            _logger = logManager.GetLogger<StoryboardDialogsService>();
        }

        [Obsolete("Use ShowDialogAsync(new ConfirmDialogConfig()) instead.")]
        public Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string? cancelButtonText = null,
            OpenDialogOptions? options = null)
        {
            return ShowDialogAsync(new ConfirmDialogConfig(
                title,
                message,
                okButtonText,
                cancelButtonText,
                options?.DialogType == DialogType.Destructive));
        }

        public virtual Task ShowDialogAsync(AlertDialogConfig config)
        {
            return new IosAlertDialog(_viewLocator, config).ShowAsync();
        }

        public virtual Task<bool> ShowDialogAsync(ConfirmDialogConfig config)
        {
            return new IosConfirmDialog(_viewLocator, config).ShowAsync();
        }

        public virtual Task<string> ShowDialogAsync(ActionSheetDialogConfig config)
        {
            return new IosActionSheetDialog(_viewLocator, config).ShowAsync();
        }

        public Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : class, IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
           IEnumerable<NavigationParameterModel>? parameters = null)
           where TViewModel : class, IDialogViewModel
        {
            try
            {
                var presentationResult = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var dismissionTask = DismissViewControllerAsync(presentationResult.ViewController);

                return new DialogResult(dismissionTask);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return default!;
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
           IEnumerable<NavigationParameterModel>? parameters = null)
           where TViewModel : class, IDialogViewModel
        {
            try
            {
                var presentationResult = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var result = presentationResult.Result is TResult convertedResult
                    ? convertedResult
                    : default!;

                var dismissionTask = DismissViewControllerAsync(presentationResult.ViewController);

                return new DialogResult<TResult>(result, dismissionTask);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return default!;
        }

        private async Task<PresentationResult> ShowViewModelForResultAsync<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _container.Resolve<TViewModel>();

            if (parameters != null)
            {
                viewModel.ApplyParameters(parameters);
            }

            var presentedViewController = await PresentModalViewControllerAsync(viewModel).ConfigureAwait(false);
            try
            {
                var dialogResult = await viewModel.DialogComponent.Task;

                return new PresentationResult(presentedViewController, dialogResult);
            }
            catch (Exception e)
            {
                _logger.Warn(e);
            }

            return new PresentationResult(presentedViewController, null);
        }

        private Task<UIViewController> PresentModalViewControllerAsync(object viewModel)
        {
            var tcs = new TaskCompletionSource<UIViewController>();

            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    var targetViewController = _viewLocator.GetView(viewModel);
                    var topViewController = _viewLocator.GetTopViewController();
                    topViewController.View?.EndEditing(true);
                    topViewController.PresentViewController(targetViewController, true, () => tcs.TrySetResult(targetViewController));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        private Task<bool> DismissViewControllerAsync(UIViewController presentedController)
        {
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    presentedController.View?.EndEditing(true);
                    presentedController.PresentingViewController
                        .DismissViewController(true, () => tcs.TrySetResult(true));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    tcs.TrySetResult(false);
                }
            });
            return tcs.Task;
        }

        private class PresentationResult
        {
            public PresentationResult(UIViewController viewController, object? result)
            {
                ViewController = viewController;
                Result = result;
            }

            public UIViewController ViewController { get; }
            public object? Result { get; }
        }
    }
}
