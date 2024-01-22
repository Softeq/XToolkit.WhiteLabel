// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.ViewModels.BottomTabs.First;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.BottomTabs.First
{
    public partial class RedViewController : ViewControllerBase<RedViewModel>
    {
        public RedViewController(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}
