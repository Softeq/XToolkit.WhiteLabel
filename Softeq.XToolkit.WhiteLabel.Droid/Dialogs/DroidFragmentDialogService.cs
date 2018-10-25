// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IAlertBuilder _alertBuilder;
        private readonly ViewLocator _viewLocator;

        public DroidFragmentDialogService(
            ViewLocator viewLocator,
            IAlertBuilder alertBuilder)
        {
            _viewLocator = viewLocator;
            _alertBuilder = alertBuilder;
        }

        public OpenDialogOptions DefaultOptions { get; } = new OpenDialogOptions();

        public Task<bool> ShowDialogAsync(string title, string message, string okButtonText, string cancelButtonText = null)
        {
            return _alertBuilder.ShowAlertAsync(title, message, okButtonText, cancelButtonText);
        }

        public Task<TViewModel> ShowForViewModel<TViewModel>(OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel
        {
            var viewModel = ServiceLocator.Resolve<TViewModel>();
            return ShowImpl<TViewModel>(viewModel as ViewModelBase);
        }

        public Task<TViewModel> ShowForViewModel<TViewModel, TParameter>(TParameter parameter,
            OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel, IViewModelParameter<TParameter>
        {
            var viewModel = ServiceLocator.Resolve<TViewModel>();
            viewModel.Parameter = parameter;
            return ShowImpl<TViewModel>(viewModel as ViewModelBase);
        }

        private Task<TViewModel> ShowImpl<TViewModel>(ViewModelBase viewModel)
            where TViewModel : class, IDialogViewModel
        {
            var fragmentBase = (DialogFragmentBase<TViewModel>)_viewLocator.GetView(viewModel, ViewType.DialogFragment);
            return fragmentBase.ShowAsync();
        }
    }
}