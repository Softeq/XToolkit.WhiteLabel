// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class ViewModelFactoryService : IViewModelFactoryService
    {
        public TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>
        {
            var viewModel = Dependencies.Container.Resolve<TViewModel>();
            viewModel.Parameter = param;
            return viewModel;
        }

        public TViewModel ResolveViewModel<TViewModel>() where TViewModel : ObservableObject
        {
            var viewModel = Dependencies.Container.Resolve<TViewModel>();
            return viewModel;
        }
    }
}