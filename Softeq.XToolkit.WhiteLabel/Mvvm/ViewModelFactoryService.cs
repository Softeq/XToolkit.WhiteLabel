// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class ViewModelFactoryService : IViewModelFactoryService
    {
        private readonly IContainer _iocContainer;

        public ViewModelFactoryService(
            IContainer container)
        {
            _iocContainer = container;
        }

        public TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.Parameter = param;
            return viewModel;
        }

        public TViewModel ResolveViewModel<TViewModel>() where TViewModel : ObservableObject
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            return viewModel;
        }
    }
}