// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class DroidFragmentDialogService : IDialogsService
    {
        private readonly IDefaultAlertBuilder _defaultAlertBuilder;
        private readonly ViewLocator _viewLocator;

        public DroidFragmentDialogService(
            ViewLocator viewLocator,
            IDefaultAlertBuilder defaultAlertBuilder)
        {
            _viewLocator = viewLocator;
            _defaultAlertBuilder = defaultAlertBuilder;
        }

        public OpenDialogOptions DefaultOptions { get; } = new OpenDialogOptions();

        public Task<bool> ShowDialogAsync(string title, string message, string okButtonText, string cancelButtonText = null)
        {
            return _defaultAlertBuilder.ShowAlertAsync(title, message, okButtonText, cancelButtonText);
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