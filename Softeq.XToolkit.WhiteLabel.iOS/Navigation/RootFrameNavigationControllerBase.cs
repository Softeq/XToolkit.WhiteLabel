// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class RootFrameNavigationControllerBase<TViewModel> : UINavigationController
        where TViewModel : RootFrameNavigationViewModelBase
    {
        public TViewModel ViewModel { get; private set; }

        public void SetExistingViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel.IsInitialized)
            {
                return;
            }

            ViewModel.InitializeNavigation(this);
            ViewModel.NavigateToFirstPage();
        }
    }
}