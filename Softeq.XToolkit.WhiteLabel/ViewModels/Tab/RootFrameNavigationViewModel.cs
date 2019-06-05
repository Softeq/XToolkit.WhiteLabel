using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public class RootFrameNavigationViewModel : RootFrameNavigationViewModelBase
    {
        private Type _viewModelType;

        public RootFrameNavigationViewModel(IFrameNavigationService frameNavigationService) : base(frameNavigationService)
        {
        }

        public int Index { get; set; }

        public override void NavigateToFirstPage()
        {
            FrameNavigationService.NavigateToViewModel(_viewModelType, true);
        }

        internal void SetViewModelTyupe(Type viewModel)
        {
            _viewModelType = viewModel;
        }
    }
}
