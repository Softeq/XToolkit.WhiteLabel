// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using ObjCRuntime;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Playground.iOS.ViewControllers.Frames
{
    [Register(nameof(SplitFrameViewController))]
    public partial class SplitFrameViewController : ViewControllerBase<SplitFrameViewModel>
    {
        public SplitFrameViewController(NativeHandle handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var rootViewController = new RootFrameNavigationControllerBase<TopShellViewModel>();
            rootViewController.NavigationBar.Hidden = true;
            ((IBindable) rootViewController).SetDataContext(ViewModel.TopShellViewModel);
            rootViewController.AddAsChildWithConstraints(this, ContentView);
        }
    }
}
