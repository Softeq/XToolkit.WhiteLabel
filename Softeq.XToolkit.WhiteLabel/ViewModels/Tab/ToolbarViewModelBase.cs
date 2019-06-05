// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Halo.Core.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        private readonly ITabNavigationService _tabNavigationService;

        public ToolbarViewModelBase(ITabNavigationService tabNavigationService, int count)
        {
            _tabNavigationService = tabNavigationService;

            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);

            GoBackCommand = new RelayCommand(GoBack);

            Dictionary = new List<RootFrameNavigationViewModel>();
           
            for(int i = 0; i < count; i++)
            {
                Dictionary.Add(Dependencies.IocContainer.Resolve<RootFrameNavigationViewModel>());
            }
        }

        public bool CanGoBack => _tabNavigationService.CanGoBack;

        public ICommand GoBackCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedIndex { get; set; }

        public List<RootFrameNavigationViewModel> Dictionary { get; }

        public abstract Type GetViewModel(int position);

        public override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var rootViewModel in Dictionary)
            {
                rootViewModel.Initialize(GetViewModel(Dictionary.IndexOf(rootViewModel)));
            }
            _tabNavigationService.SetSelectedViewModel(Dictionary[SelectedIndex]);
        }

        private void SelectionChanged(int selectedViewModel)
        {
            if (Equals(SelectedIndex, selectedViewModel))
            {
                return;
            }

            SelectedIndex = selectedViewModel;
            _tabNavigationService.SetSelectedViewModel(Dictionary[SelectedIndex]);
        }

        private void GoBack()
        {
            _tabNavigationService.GoBack();
        }
    }
}