// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.BottomTabs.Second
{
    public partial class BlueViewController : ViewControllerBase<BlueViewModel>
    {
        public BlueViewController(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}
