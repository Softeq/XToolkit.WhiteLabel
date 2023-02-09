// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using ObjCRuntime;
using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.BottomTabs.Second
{
    public partial class GreenViewController : ViewControllerBase<GreenViewModel>
    {
        public GreenViewController(NativeHandle handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Button.SetCommand(ViewModel.IncrementCommand);
        }
    }
}
