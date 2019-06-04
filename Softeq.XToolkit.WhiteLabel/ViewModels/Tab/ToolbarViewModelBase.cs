// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Halo.Core.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        public ToolbarViewModelBase(int count)
        {
            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);

            Dictionary = new List<RootFrameNavigationViewModel>();
           
            for(int i = 0; i < count; i++)
            {
                Dictionary.Add(Dependencies.IocContainer.Resolve<RootFrameNavigationViewModel>());
            }
        }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedViewModel { get; set; }

        public List<RootFrameNavigationViewModel> Dictionary { get; }

        public abstract Type GetViewModel(int position);

        public override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var rootViewModel in Dictionary)
            {
                rootViewModel.Initialize(GetViewModel(Dictionary.IndexOf(rootViewModel)));
            }
        }

        private void SelectionChanged(int selectedViewModel)
        {
            if (Equals(SelectedViewModel, selectedViewModel))
            {
                return;
            }

            SelectedViewModel = selectedViewModel;
        }
    }
}