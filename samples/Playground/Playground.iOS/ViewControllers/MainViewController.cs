using System;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers
{
    public partial class MainViewController : ViewControllerBase<MainViewModel>
    {
        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        partial void OnCollectionViewTapped(Foundation.NSObject sender)
        {
            ViewModel.NavigateToCollectionsSample();
        }
    }
}
