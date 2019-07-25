// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class RootFrameNavigationControllerBase<TViewModel> : UINavigationController, IBindableOwner
        where TViewModel : RootFrameNavigationViewModelBase
    {
        protected RootFrameNavigationControllerBase()
        {
            Bindings = new List<Binding>();
        }

        public TViewModel ViewModel { get; private set; }
        public List<Binding> Bindings { get; }

        public void SetExistingViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel.OnInitialize();
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
