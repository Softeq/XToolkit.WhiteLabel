// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        private readonly ITabNavigationService _tabNavigationService;
        private readonly IContainer _container;

        protected ToolbarViewModelBase(
            ITabNavigationService tabNavigationService,
            IContainer container)
        {
            _tabNavigationService = tabNavigationService;
            _container = container;

            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);
            GoBackCommand = new RelayCommand(GoBack);
        }

        public bool CanGoBack => _tabNavigationService.CanGoBack;

        public IList<TabItem> TabModels { get; protected set; }

        public IList<RootFrameNavigationViewModel> TabViewModels { get; private set; }

        public ICommand GoBackCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedIndex { get; private set; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            if (TabModels == null)
            {
                throw new Exception("You must init TabModels property");
            }

            TabViewModels = TabModels
                .Select(CreateRootViewModel)
                .ToList();

            var selectedViewModel = TabViewModels[SelectedIndex];

            _tabNavigationService.SetSelectedViewModel(selectedViewModel);
        }

        // TODO YP: review to rework via bindings
        private void SelectionChanged(int selectedIndex)
        {
            if (Equals(SelectedIndex, selectedIndex))
            {
                return;
            }

            SelectedIndex = selectedIndex;

            var selectedViewModel = TabViewModels[SelectedIndex];

            _tabNavigationService.SetSelectedViewModel(selectedViewModel);
        }

        private void GoBack()
        {
            _tabNavigationService.GoBack();
        }

        private RootFrameNavigationViewModel CreateRootViewModel(TabItem tabModel)
        {
            var viewModel = _container.Resolve<RootFrameNavigationViewModel>();

            viewModel.Initialize(tabModel);

            return viewModel;
        }
    }
}
