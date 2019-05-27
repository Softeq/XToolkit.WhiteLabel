// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.iOS;
using Playground.ViewModels.Pages;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class StartPageViewController
        : ViewControllerBase<StartPageViewModel>
    {
        public StartPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            // TODO YP: need another way to register starting navigation
            var viewModel = Dependencies.IocContainer.Resolve<StartPageViewModel>();
            SetExistingViewModel(viewModel);

            base.ViewDidLoad();
        }
    }
}

