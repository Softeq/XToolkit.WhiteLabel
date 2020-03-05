// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;
using Softeq.XToolkit.WhiteLabel.Extensions;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardDialogsService : IDialogsService
    {
        private readonly ILogger _logger;
        private readonly IViewLocator _viewLocator;
        private readonly IContainer _iocContainer;

        public StoryboardDialogsService(
            IViewLocator viewLocator,
            ILogManager logManager,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
            _logger = logManager.GetLogger<StoryboardDialogsService>();
        }

        public Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string? cancelButtonText = null,
            OpenDialogOptions? options = null)
        {
            var dialogResult = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

                var okActionStyle = options?.DialogType == DialogType.Destructive
                    ? UIAlertActionStyle.Destructive
                    : UIAlertActionStyle.Default;

                alertController.AddAction(UIAlertAction.Create(okButtonText, okActionStyle,
                    action => { dialogResult.TrySetResult(true); }));

                if (cancelButtonText != null)
                {
                    alertController.AddAction(UIAlertAction.Create(cancelButtonText, UIAlertActionStyle.Cancel,
                        action => { dialogResult.TrySetResult(false); }));
                }

                var topViewController = _viewLocator.GetTopViewController();
                if (topViewController == null)
                {
                    _logger.Error("can't find top ViewController");
                    dialogResult.TrySetResult(false);
                    return;
                }

                topViewController.PresentViewController(alertController, true, null);
            });

            return dialogResult.Task;
        }

        public Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel>? parameters = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
           IEnumerable<NavigationParameterModel>? parameters = null)
           where TViewModel : IDialogViewModel
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
           where TViewModel : IDialogViewModel
        {
            try
            {
                var presentationResult = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var result = presentationResult.Result is TResult convertedResult
                    ? convertedResult
                    : default;

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
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
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

        private Task<PresentedViewController> PresentModalViewControllerAsync(object viewModel)
        {
            var tcs = new TaskCompletionSource<PresentedViewController>();

            Execute.BeginOnUIThread(() =>
            {
                var targetViewController = _viewLocator.GetView(viewModel);
                var topViewController = _viewLocator.GetTopViewController();
                topViewController.View.EndEditing(true);
                topViewController.PresentViewController(targetViewController, true, null);
                tcs.TrySetResult(new PresentedViewController(topViewController, targetViewController));
            });

            return tcs.Task;
        }

        private Task<bool> DismissViewControllerAsync(PresentedViewController controllerRequest)
        {
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    controllerRequest.Modal.View.EndEditing(true);
                    controllerRequest.Parent.DismissViewController(true, () => tcs.TrySetResult(true));
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
            public PresentationResult(PresentedViewController viewController, object? result)
            {
                ViewController = viewController;
                Result = result;
            }

            public PresentedViewController ViewController { get; }
            public object? Result { get; }
        }

        private class PresentedViewController
        {
            public PresentedViewController(UIViewController parent, UIViewController modal)
            {
                Parent = parent;
                Modal = modal;
            }

            public UIViewController Parent { get; }
            public UIViewController Modal { get; }
        }
    }
}
