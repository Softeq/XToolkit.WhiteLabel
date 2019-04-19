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

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IAlertBuilder _alertBuilder;
        private readonly IIocContainer _iocContainer;
        private readonly IViewLocator _viewLocator;

        public DroidFragmentDialogService(
            IViewLocator viewLocator,
            IAlertBuilder alertBuilder, IIocContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _alertBuilder = alertBuilder;
            _iocContainer = iocContainer;
        }

        public OpenDialogOptions DefaultOptions { get; } = new OpenDialogOptions();

        public Task<bool> ShowDialogAsync(string title, string message, string okButtonText,
            string cancelButtonText = null, OpenDialogOptions openDialogOptions = null)
        {
            return _alertBuilder.ShowAlertAsync(title, message, okButtonText, cancelButtonText);
        }

        public Task<TViewModel> ShowForViewModel<TViewModel>(OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            return ShowImpl<TViewModel>(viewModel as ViewModelBase);
        }

        public Task<TViewModel> ShowForViewModel<TViewModel, TParameter>(TParameter parameter,
            OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel, IViewModelParameter<TParameter>
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.Parameter = parameter;
            return ShowImpl<TViewModel>(viewModel as ViewModelBase);
        }

        public Task<IDialogViewModel> ShowForViewModel<TViewModel>(IEnumerable<NavigationParameterModel> parameters) 
            where TViewModel : class, IDialogViewModel
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);

            return viewModel.DialogComponent.Task;
        }

        public async Task<TResult> ShowForViewModel<TViewModel, TResult>(IEnumerable<NavigationParameterModel> parameters) 
            where TViewModel : class, IDialogViewModel
        {
            var result = default(TResult);

            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);

            var resultObject = await viewModel.DialogComponent.TaskWithResult.ConfigureAwait(false);

            if (resultObject is TResult tResult)
            {
                result = tResult;
            }

            return result;
        }

        private Task<TViewModel> ShowImpl<TViewModel>(ViewModelBase viewModel)
            where TViewModel : class, IDialogViewModel
        {
            var fragmentBase = (DialogFragmentBase<TViewModel>)_viewLocator.GetView(viewModel, ViewType.DialogFragment);
            return fragmentBase.ShowAsync();
        }
    }
}