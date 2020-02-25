// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.Forms.ViewModels.MasterDetailNavigation.DrillNavigation;
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

        public IReadOnlyList<string> Keys { get; } = new List<string>
        {
            "root",
            "drill-navigation",
            "item"
        };

        public IViewModelBase GetViewModelByKey(string key)
        {
            switch (key)
            {
                case "root":
                    return _viewModelFactoryService.ResolveViewModel<DetailViewModel>();
                case "drill-navigation":
                    return _viewModelFactoryService.ResolveViewModel<DrillRootPageViewModel>();
                default:
                    var vm = _viewModelFactoryService.ResolveViewModel<SelectedItemViewModel>();
                    vm.Title = key;
                    return vm;
            }
        }
    }
}
