// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
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

        public async Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            IDialogResult<TResult> dialogResult = null;

            try
            {
                var (viewController, resultObject) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                var result = resultObject is TResult convertedResult
                    ? convertedResult
                    : default(TResult);

                dialogResult = new DialogResult<TResult>(result, DismissViewController(viewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return dialogResult;
        }

        public async Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            IDialogResult dialogResult = null;

            try
            {
                var (viewController, result) = await ShowViewModelForResultAsync<TViewModel>(parameters);

                dialogResult = new DialogResult(DismissViewController(viewController));
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return dialogResult;
        }

        public Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel>(parameters).WaitUntilDissmissed();
        }

        public Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            return ShowForViewModelAsync<TViewModel, TResult>(parameters).ReturnWhenDissmissed();
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
                var alertController = new TopLevelAlertController
                {
                    Title = title,
                    Message = message
                };

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

                alertController.Show();
            });

            return dialogResult.Task;
        }

        private async Task<(UIViewController, object)> ShowViewModelForResultAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            var viewController = await PresentModalViewController(viewModel).ConfigureAwait(false);
            var dialogResult = await viewModel.DialogComponent.Task;

            return (viewController, dialogResult);
        }

        private Task<UIViewController> PresentModalViewController(object viewModel)
        {
            var source = new TaskCompletionSource<UIViewController>();

            Execute.BeginOnUIThread(() =>
            {
                var controller = _viewLocator.GetView(viewModel);
                controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                var viewController = _viewLocator.GetTopViewController();
                viewController.View.EndEditing(true);
                viewController.PresentViewController(controller, true, null);
                source.TrySetResult(viewController);
            });

            return source.Task;
        }

        private TaskCompletionSource<bool> DismissViewController(UIViewController viewController)
        {
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
            {
                try
                {
                    viewController.ModalViewController.View.EndEditing(true);
                    viewController.DismissViewController(true, () => tcs.TrySetResult(true));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    tcs.TrySetResult(false);
                }
            });
            return tcs;
        }
    }
}
