// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.iOS.ViewControllers
{
    [Register(nameof(RootNavigationController))]
    public class RootNavigationController : UINavigationController
    {
        public RootNavigationController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var navigationService = Dependencies.IocContainer.Resolve<IPageNavigationService>();
            navigationService.Initialize(this);
        }
    }
}
