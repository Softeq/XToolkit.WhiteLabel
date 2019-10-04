// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        private readonly ITabNavigationService _tabNavigationService;
        private readonly IContainer _iocContainer;

        protected ToolbarViewModelBase(
            ITabNavigationService tabNavigationService,
            IContainer iocContainer)
        {
            _tabNavigationService = tabNavigationService;
            _iocContainer = iocContainer;

            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);
            GoBackCommand = new RelayCommand(GoBack);
        }

        public bool CanGoBack => _tabNavigationService.CanGoBack;

        public IList<TabItem> TabModels { get; protected set; }

        public IList<TabViewModel> TabViewModels { get; protected set; }

        public ICommand GoBackCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedIndex { get; private set; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            if (TabModels == null)
            {
                throw new InvalidOperationException($"You must init {nameof(TabModels)} property");
            }

            TabViewModels = TabModels
                .Select(CreateTabViewModel)
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

        private TabViewModel CreateTabViewModel(TabItem tabModel)
        {
            var tabViewModel = _iocContainer.Resolve<TabViewModel>();

            tabViewModel.Initialize(tabModel);

            return tabViewModel;
        }
    }
}
