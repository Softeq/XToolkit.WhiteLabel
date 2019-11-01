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
                var (modalViewController, topViewController, result) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                return new DialogResult(DismissViewControllerAsync(modalViewController, topViewController));
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
                var (modalViewController, topViewController, resultObject) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var result = resultObject is TResult convertedResult
                    ? convertedResult
                    : default;

                return new DialogResult<TResult>(result, DismissViewControllerAsync(modalViewController, topViewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            return null;
        }

        private async Task<(UIViewController, UIViewController, object)> ShowViewModelForResultAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            var result = await PresentModalViewControllerAsync(viewModel).ConfigureAwait(false);
            try
            {
                var dialogResult = await viewModel.DialogComponent.Task;

                return (result.ModalVC, result.ParentVC, dialogResult);
            }
            catch (Exception ex)
            {
                return (result.ModalVC, result.ParentVC, null);
            }
        }

        private Task<(UIViewController ModalVC, UIViewController ParentVC)> PresentModalViewControllerAsync(object viewModel)
        {
            var tcs = new TaskCompletionSource<(UIViewController, UIViewController)>();

            Execute.BeginOnUIThread(() =>
            {
                var targetViewController = _viewLocator.GetView(viewModel);
                targetViewController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                var topViewController = _viewLocator.GetTopViewController();
                topViewController.View.EndEditing(true);
                topViewController.PresentViewController(targetViewController, true, null);
                tcs.TrySetResult((targetViewController, topViewController));
            });

            return tcs.Task;
        }

        private Task<bool> DismissViewControllerAsync(UIViewController modalViewController, UIViewController parentViewController)
        {
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    modalViewController.View.EndEditing(true);
                    parentViewController.DismissViewController(true, () => tcs.TrySetResult(true));
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
