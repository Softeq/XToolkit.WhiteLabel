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

            Keys = new List<string>
            {
                MasterDetailItems.Root,
                MasterDetailItems.Drill,
                MasterDetailItems.Item
            };
        }

        public IReadOnlyList<string> Keys { get; }

        public IViewModelBase GetViewModelByKey(string key)
        {
            switch (key)
            {
                case MasterDetailItems.Root:
                    return _viewModelFactoryService.ResolveViewModel<DetailPageViewModel>();
                case MasterDetailItems.Drill:
                    return _viewModelFactoryService.ResolveViewModel<DrillRootPageViewModel>();
                default:
                    var vm = _viewModelFactoryService.ResolveViewModel<SelectedItemPageViewModel>();
                    vm.Title = key;
                    return vm;
            }
        }

        private static class MasterDetailItems
        {
            public const string Root = "Root";
            public const string Drill = "Drill Navigation";
            public const string Item = "Item";
        }
    }
}
