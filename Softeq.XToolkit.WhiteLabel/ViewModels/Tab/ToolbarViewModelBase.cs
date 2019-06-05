// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        private readonly ITabNavigationService _tabNavigationService;

        public ToolbarViewModelBase(ITabNavigationService tabNavigationService, int count)
        {
            _tabNavigationService = tabNavigationService;

            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);

            GoBackCommand = new RelayCommand(GoBack);

            TabViewModels = new List<RootFrameNavigationViewModel>();
           
            for (int i = 0; i < count; i++)
            {
                TabViewModels.Add(Dependencies.IocContainer.Resolve<RootFrameNavigationViewModel>());
            }
        }

        public bool CanGoBack => _tabNavigationService.CanGoBack;

        public ICommand GoBackCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedIndex { get; set; }

        public List<RootFrameNavigationViewModel> TabViewModels { get; }

        protected abstract Type GetViewModel(int position);

        public override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var rootViewModel in TabViewModels)
            {
                var index = TabViewModels.IndexOf(rootViewModel);
                rootViewModel.SetViewModelType(GetViewModel(index));
            }
            _tabNavigationService.SetSelectedViewModel(TabViewModels[SelectedIndex]);
        }

        private void SelectionChanged(int selectedViewModel)
        {
            if (Equals(SelectedIndex, selectedViewModel))
            {
                return;
            }

            SelectedIndex = selectedViewModel;
            _tabNavigationService.SetSelectedViewModel(TabViewModels[SelectedIndex]);
        }

        private void GoBack()
        {
            _tabNavigationService.GoBack();
        }
    }
}