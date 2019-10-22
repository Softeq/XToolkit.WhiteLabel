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
using CoreGraphics;
using CoreAnimation;
using Foundation;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardDialogsService : IDialogsService
    {
        private readonly ILogger _logger;
        private readonly IViewLocator _viewLocator;
        private readonly IContainer _iocContainer;
        private readonly IosPresentationStyleStorage _iosPresentationStyleStorage;

        public StoryboardDialogsService(
            IosPresentationStyleStorage iosPresentationStyleStorage,
            IViewLocator viewLocator,
            ILogManager logManager,
            IContainer iocContainer)
        {
            _iosPresentationStyleStorage = iosPresentationStyleStorage;
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
            IEnumerable<NavigationParameterModel> parameters = null,
            string presentationStyleId = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDismissed();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null,
            string presentationStyleId = null)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).WaitUntilDismissed();
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null,
            string presentationStyleId = null)
           where TViewModel : IDialogViewModel
        {
            try
            {
                var (viewController, result) = await ShowViewModelForResultAsync<TViewModel>(parameters, presentationStyleId);

                return new DialogResult(DismissViewControllerAsync(viewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            return null;
        }

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null,
            string presentationStyleId = null)
           where TViewModel : IDialogViewModel
        {
            try
            {
                var (viewController, resultObject) = await ShowViewModelForResultAsync<TViewModel>(parameters, presentationStyleId);

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
            IEnumerable<NavigationParameterModel> parameters,
            string presentationStyleId)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            var viewController = await PresentModalViewControllerAsync(viewModel, presentationStyleId).ConfigureAwait(false);
            var dialogResult = await viewModel.DialogComponent.Task;

            return (viewController, dialogResult);
        }

        private Task<UIViewController> PresentModalViewControllerAsync(object viewModel, string presentationStyleId)
        {
            var tcs = new TaskCompletionSource<UIViewController>();
            PresentationArgsBase args = null;

            if(presentationStyleId != null)
            {
                args = _iosPresentationStyleStorage.GetStyle(presentationStyleId);
            }

            Execute.BeginOnUIThread(() =>
            {
                var targetViewController = _viewLocator.GetView(viewModel);
                targetViewController.ModalPresentationStyle = args?.PresentationStyle ?? UIModalPresentationStyle.OverFullScreen;
                targetViewController.TransitioningDelegate = args?.TransitioningDelegate;
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
