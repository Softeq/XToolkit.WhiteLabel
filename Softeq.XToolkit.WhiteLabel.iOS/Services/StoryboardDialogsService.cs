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
            string cancelButtonText = null,
            OpenDialogOptions options = null)
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
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
           IEnumerable<NavigationParameterModel> parameters = null)
           where TViewModel : IDialogViewModel
        {
            try
            {
                var (viewController, result) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                return new DialogResult(DismissViewControllerAsync(viewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            return null;
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
           IEnumerable<NavigationParameterModel> parameters = null)
           where TViewModel : IDialogViewModel
        {
            try
            {
                var (viewController, resultObject) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var result = resultObject is TResult convertedResult
                    ? convertedResult
                    : default;

                return new DialogResult<TResult>(result, DismissViewControllerAsync(viewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            return null;
        }

        private async Task<(UIViewController, object)> ShowViewModelForResultAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            var viewController = await PresentModalViewControllerAsync(viewModel).ConfigureAwait(false);
            var dialogResult = await viewModel.DialogComponent.Task;

            return (viewController, dialogResult);
        }

        private Task<UIViewController> PresentModalViewControllerAsync(object viewModel)
        {
            var tcs = new TaskCompletionSource<UIViewController>();

            Execute.BeginOnUIThread(() =>
            {
                var targetViewController = _viewLocator.GetView(viewModel);
                targetViewController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                var topViewController = _viewLocator.GetTopViewController();
                topViewController.View.EndEditing(true);
                topViewController.PresentViewController(targetViewController, true, null);
                tcs.TrySetResult(targetViewController);
            });

            return tcs.Task;
        }

        private Task<bool> DismissViewControllerAsync(UIViewController viewController)
        {
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    viewController.View.EndEditing(true);
                    viewController.DismissViewController(true, () => tcs.TrySetResult(true));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    tcs.TrySetResult(false);
                }
            });
            return tcs.Task;
        }
    }
}
