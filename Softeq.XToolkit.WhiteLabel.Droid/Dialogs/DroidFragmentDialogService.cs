// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IAlertBuilder _alertBuilder;
        private readonly IIocContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        public DroidFragmentDialogService(
            IViewLocator viewLocator,
            IAlertBuilder alertBuilder,
            IIocContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _alertBuilder = alertBuilder;
            _iocContainer = iocContainer;
        }

        public DialogNavigationHelper<TViewModel> For<TViewModel>() where TViewModel : IDialogViewModel
        {
            return new DialogNavigationHelper<TViewModel>(this);
        }

        public Task<bool> ShowDialogAsync(string title, string message, string okButtonText,
            string cancelButtonText = null, OpenDialogOptions openDialogOptions = null)
        {
            return _alertBuilder.ShowAlertAsync(title, message, okButtonText, cancelButtonText);
        }

        public async Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            var result = default(TResult);
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);

            ShowImpl(viewModel);

            var resultObject = await viewModel.DialogComponent.Task;

            if (resultObject is TResult tResult)
            {
                result = tResult;
            }

            return result;
        }

        public async Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);

            ShowImpl(viewModel);

            await viewModel.DialogComponent.Task;
        }

        private void ShowImpl<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
        {
            var fragmentBase = (DialogFragmentBase<TViewModel>) _viewLocator.GetView(viewModel, ViewType.DialogFragment);
            fragmentBase.Show();
        }
    }
}