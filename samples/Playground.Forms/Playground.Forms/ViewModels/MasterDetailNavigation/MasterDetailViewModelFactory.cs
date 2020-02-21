// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.MasterDetailNavigation
{
    public class MasterDetailViewModelFactory
    {
        private readonly IViewModelFactoryService _viewModelFactoryService;

        public MasterDetailViewModelFactory(IViewModelFactoryService viewModelFactoryService)
        {
            _viewModelFactoryService = viewModelFactoryService;
        }

        public IViewModelBase GetViewModelByKey(string key)
        {
            if (key == "root")
            {
                return _viewModelFactoryService.ResolveViewModel<DetailViewModel>();
            }
            else
            {
                var vm = _viewModelFactoryService.ResolveViewModel<SelectedItemViewModel>();
                vm.Title = key;
                return vm;
            }
        }
    }
}
