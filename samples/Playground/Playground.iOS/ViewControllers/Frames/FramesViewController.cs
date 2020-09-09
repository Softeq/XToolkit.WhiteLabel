// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Playground.iOS.ViewControllers.Frames
{
    [Register(nameof(FramesViewController))]
    public partial class FramesViewController : ViewControllerBase<FramesViewModel>
    {
        public FramesViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var rootViewController = new RootFrameNavigationControllerBase<FramesViewModel>();
            rootViewController.NavigationBar.Hidden = true;
            ((IBindable) rootViewController).SetDataContext(ViewModel);
            rootViewController.AddAsChildWithConstraints(this, ContentView);
        }
    }
}
