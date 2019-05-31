// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
    {
        public MainPageViewController(IntPtr handle) : base(handle)
        {
        }

        partial void OnCollectionViewTapped(Foundation.NSObject sender)
        {
            ViewModel.NavigateToCollectionsSample();
        }
    }
}
