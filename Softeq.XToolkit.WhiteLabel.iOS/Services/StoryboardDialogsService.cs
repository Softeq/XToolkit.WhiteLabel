﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardDialogsService : IDialogsService
    {
        private readonly ILogger _logger;
        private readonly IViewLocator _viewLocator;
        private readonly IIocContainer _iocContainer;

        public StoryboardDialogsService(IViewLocator viewLocator, ILogManager logManager, IIocContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
            _logger = logManager.GetLogger<StoryboardDialogsService>();
        }

        public OpenDialogOptions DefaultOptions { get; } = new OpenDialogOptions {ShouldShowBackgroundOverlay = false};

        public async Task<TViewModel> ShowForViewModel<TViewModel>(OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel
        {
            IDialogViewModel result = null;
            try
            {
                var viewModel = _iocContainer.Resolve<TViewModel>();
                var viewController = await PresentModalViewController(viewModel).ConfigureAwait(false);
                result = await GetResultAndDismiss(viewModel, viewController);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return result as TViewModel;
        }

        public async Task<TViewModel> ShowForViewModel<TViewModel, TParameter>(TParameter parameter,
            OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel, IViewModelParameter<TParameter>
        {
            IDialogViewModel result = null;
            try
            {
                var viewModel = _iocContainer.Resolve<TViewModel>();
                var viewModelWithParameter = (IViewModelParameter<TParameter>) viewModel;
                viewModelWithParameter.Parameter = parameter;
                var viewController = await PresentModalViewController(viewModel).ConfigureAwait(false);
                result = await GetResultAndDismiss(viewModel, viewController);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return result as TViewModel;
        }

        public Task<bool> ShowDialogAsync(string title,
                                          string message,
                                          string okButtonText,
                                          string cancelButtonText = null,
                                          bool isDestructiveConfirmation = false)
        {
            var dialogResult = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                var okActionStyle = isDestructiveConfirmation ? UIAlertActionStyle.Destructive : UIAlertActionStyle.Default;
                alertController.AddAction(UIAlertAction.Create(okButtonText, okActionStyle,
                    action => { dialogResult.TrySetResult(true); }));

                if (cancelButtonText != null)
                {
                    alertController.AddAction(UIAlertAction.Create(cancelButtonText, UIAlertActionStyle.Cancel,
                        action => { dialogResult.TrySetResult(false); }));
                }

                var viewController = _viewLocator.GetTopViewController();
                viewController.PresentViewController(alertController, true, null);
            });

            return dialogResult.Task;
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

        private static async Task<IDialogViewModel> GetResultAndDismiss<TViewModel>(TViewModel viewModel,
            UIViewController viewController) where TViewModel : class, IDialogViewModel
        {
            var result = await viewModel.DialogComponent.Task.ConfigureAwait(false);
            var tcs = new TaskCompletionSource<bool>();
            Execute.BeginOnUIThread(() =>
                viewController.DismissViewController(true, () => { tcs.TrySetResult(true); }));
            await tcs.Task;
            return result;
        }
    }
}