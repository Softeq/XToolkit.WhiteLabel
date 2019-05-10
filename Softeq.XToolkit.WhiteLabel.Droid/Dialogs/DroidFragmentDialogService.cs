// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IAlertBuilder _alertBuilder;
        private readonly IIocContainer _iocContainer;
        private readonly ILogger _logger;
        private readonly ViewLocator _viewLocator;

        public DroidFragmentDialogService(
            ViewLocator viewLocator,
            IAlertBuilder alertBuilder,
            IIocContainer iocContainer,
            ILogger logger)
        {
            _viewLocator = viewLocator;
            _alertBuilder = alertBuilder;
            _iocContainer = iocContainer;
            _logger = logger;
        }

        public OpenDialogOptions DefaultOptions { get; } = new OpenDialogOptions();

        public DialogNavigationHelper<T> For<T>() where T : IDialogViewModel
        {
            return new DialogNavigationHelper<T>(this);
        }

        public Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string cancelButtonText = null,
            OpenDialogOptions options = null)
        {
            return _alertBuilder.ShowAlertAsync(title, message, okButtonText, cancelButtonText);
        }

        public async Task<TResult> ShowForViewModel<TViewModel, TResult>
            (IEnumerable<NavigationParameterModel> parameters = null) where TViewModel : IDialogViewModel
        {
            var result = default(TResult);

            try
            {
                var viewModel = _iocContainer.Resolve<TViewModel>();
                viewModel.ApplyParameters(parameters);
                var updatedViewModel = await ShowImpl<TViewModel>(viewModel as ViewModelBase);
                var resultObject = await updatedViewModel.DialogComponent.Task;
                if (resultObject is TResult tResult)
                {
                    result = tResult;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }
            return result;
        }

        public async Task ShowForViewModel<TViewModel>() where TViewModel : IDialogViewModel
        {
            await ShowForViewModel<TViewModel, bool>().ConfigureAwait(false);
        }

        private Task<TViewModel> ShowImpl<TViewModel>(ViewModelBase viewModel)
            where TViewModel : IDialogViewModel
        {
            var fragmentBase = (DialogFragmentBase<TViewModel>)_viewLocator.GetView(viewModel, ViewType.DialogFragment);
            return fragmentBase.ShowAsync();
        }
    }
}