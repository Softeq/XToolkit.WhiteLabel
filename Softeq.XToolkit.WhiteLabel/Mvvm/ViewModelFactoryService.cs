// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class ViewModelFactoryService : IViewModelFactoryService
    {
        private readonly IContainer _container;

        public ViewModelFactoryService(
            IContainer container)
        {
            _container = container;
        }

        public TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>
        {
            var viewModel = _container.Resolve<TViewModel>();
            viewModel.Parameter = param;
            return viewModel;
        }

        public TViewModel ResolveViewModel<TViewModel>() where TViewModel : ObservableObject
        {
            var viewModel = _container.Resolve<TViewModel>();
            return viewModel;
        }
    }
}